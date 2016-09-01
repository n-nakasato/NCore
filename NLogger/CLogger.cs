using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NLogger
{
    /// <summary>
    /// log4netを使用したロガークラス
    /// </summary>
    public class CLogger
    {
        private int _LogFileMax = 5;
        private string _LogMatchPattern = "Trace_*.txt";
        private log4net.ILog _Logger;
        private ulong _LogId;
        private FileSystemWatcher _FSW = new FileSystemWatcher();

        /// <summary>
        /// ロガーを生成する
        /// </summary>
        /// <param name="configPath">log4net構成情報ファイルのパス</param>
        /// <param name="key">環境変数名</param>
        /// <param name="dir">ログファイルの増加を監視するパス</param>
        /// <param name="logFileMax">ログファイルの最大数</param>
        /// <param name="logMatchPtn">ログファイル名の検索パターン(例：～_*.txt)</param>
        public void CreateLogger(string configPath, string key, string dir, int logFileMax, string logMatchPtn)
        {
            // アプリケーションログの保存先パスを設定
            Environment.SetEnvironmentVariable(key, dir);

            // log4netの構成を変更する
            log4net.Config.XmlConfigurator.Configure(new FileInfo(configPath));

            // ロガーオブジェクト生成
            this._Logger = log4net.LogManager.GetLogger(log4net.LogManager.GetRepository().Name);
            this._LogId = 1;

            this._LogFileMax = logFileMax;
            this._LogMatchPattern = logMatchPtn;

            this.RemoveLogFile(dir);

            this._FSW.Path = dir;
            this._FSW.Created += _FSW_Created;
            this._FSW.EnableRaisingEvents = true;
        }

        /// <summary>
        /// ロガーを削除する
        /// </summary>
        public void DeleteLogger()
        {
            this._Logger = null;
            log4net.LogManager.Shutdown();
        }

        /// <summary>
        /// ログ文字列を取得する
        /// </summary>
        /// <param name="file"></param>
        /// <param name="line"></param>
        /// <param name="member"></param>
        /// <param name="type"></param>
        /// <param name="dst"></param>
        /// <param name="src"></param>
        /// <param name="seqNo"></param>
        /// <param name="direction"></param>
        /// <param name="msg"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public string GetString(string file, int line, string member, string type, string dst, string src, ulong seqNo, string direction, string msg, string param)
        {
            string ret = "";

            // Type Dst Src SeqName Direction Name Param
            ret = string.Format("\t{0}#{1}\t{2}\tSEQ\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}", Path.GetFileName(file), line, member, type, dst, src, seqNo, direction, msg, param);

            return ret;
        }

        /// <summary>
        /// INFOレベルのログを残す
        /// </summary>
        /// <param name="dst"></param>
        /// <param name="src"></param>
        /// <param name="type"></param>
        /// <param name="direction"></param>
        /// <param name="msg"></param>
        /// <param name="param"></param>
        /// <param name="file"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public ulong SaveInfo(
            string dst,
            string src,
            string type,
            string direction,
            string msg,
            string param = "",
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0,
            [CallerMemberName] string member = "")
        {
            ulong seqNo = this.GetSeqId();

            if (null != this._Logger)
            {
                string log = GetString(file, line, member, type, dst, src, seqNo, direction, msg, param);

                this._Logger.Info(log);
            }

            return seqNo;
        }

        /// <summary>
        /// ERRORレベルのログを残す
        /// </summary>
        /// <param name="src"></param>
        /// <param name="msg"></param>
        /// <param name="param"></param>
        /// <param name="file"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public ulong SaveError(
            string dst,
            string src,
            string type,
            string direction,
            string msg,
            string param = "",
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0, 
            [CallerMemberName] string member = "")
        {
            ulong seqNo = this.GetSeqId();

            if (null != this._Logger)
            {
                string log = GetString(file, line, member, type, dst, src, seqNo, direction, msg, param);

                this._Logger.Info(log);
            }

            return seqNo;
        }

        /// <summary>
        /// ログファイルが増えすぎないように削除する
        /// </summary>
        /// <param name="path"></param>
        private void RemoveLogFile(string path)
        {
            this.RemoveLogFile(path, _LogMatchPattern, _LogFileMax);
        }

        /// <summary>
        /// ログファイルが増えすぎないように削除する
        /// </summary>
        /// <param name="path"></param>
        /// <param name="ptn"></param>
        /// <param name="fileMax"></param>
        private void RemoveLogFile(string path, string ptn, int fileMax)
        {
            string[] files = Directory.GetFiles(path, ptn);
            string oldFile = "";
            DateTime oldFileCre = DateTime.MaxValue;
            DateTime nowFileCre;

            // 最古のログから削除する
            int cnt = files.Length - fileMax;
            while (0 < cnt)
            {
                foreach (string now in files)
                {
                    nowFileCre = File.GetCreationTime(now);
                    if (oldFileCre > nowFileCre)
                    {
                        oldFileCre = nowFileCre;
                        oldFile = now;
                    }
                }

                if ("" != oldFile)
                {
                    File.Delete(oldFile);
                }

                cnt--;
            }
        }

        /// <summary>
        /// ファイルが作成されたときにログファイルの個数をチェックする
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _FSW_Created(object sender, FileSystemEventArgs e)
        {
            lock (this)
            {
                this.RemoveLogFile(this._FSW.Path);
            }
        }

        /// <summary>
        /// シーケンス番号を取得する
        /// </summary>
        /// <returns></returns>
        protected ulong GetSeqId()
        {
            lock (this)
            {
                this._LogId++;
            }

            return this._LogId;
        }
    }
}
