using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.IO;
using System.Web;
using System.Net;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Resources;
namespace Joe.Utils
{
    class CSV
    {
        /// <summary>
        /// Saves a Comma Separated Values (CVS) File
        /// </summary>
        /// <param name="newfilename">Full path and filename</param>
        /// <param name="StrBuilder">Stringbuilder with csv format</param>
        /// <returns></returns>
        public static bool CreateCSVFile(string newfilename, StringBuilder StrBuilder)
        {
            try
            {
                FileStream fs = File.Create(newfilename);
                fs.Close();

                using (StreamWriter sw = new StreamWriter(newfilename))
                {
                    // Add some text to the file.
                    sw.Write(StrBuilder.ToString());
                    sw.Close();
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

    }

    class Helper
    {
        public static string TmpFilename;

        /// <summary>
        /// Creates a Text File
        /// </summary>
        public static void CreateTextFile(string path_filename, Stream rcvStream)
        {
            byte[] respBytes = new byte[256];
            int byteCount;

            FileStream fs = new FileStream(path_filename, FileMode.Create, FileAccess.Write);
            do
            {
                byteCount = rcvStream.Read(respBytes, 0, 256);
                fs.Write(respBytes, 0, byteCount);
            } while (byteCount > 0);

            fs.Close();
            rcvStream.Close();
        }

        /// <summary>
        /// Creates file from a webpage
        /// </summary>
        public static void DownloadWebpage(string url, string new_path_filename, string extention)
        {
            //Create Temp File path and name Saves to current system's temp folder
            if (extention.IndexOf(".") != -1)
                TmpFilename = new_path_filename + extention;
            else
                TmpFilename = new_path_filename + "." + extention;

            //MessageBox.Show(TmpFilename);

            //Get the url and make sure it is not https
            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
            {
                url = "http://" + url;
                //TxtUrl.Text = url;
            }

            //Create the HTTP Connection
            HttpWebRequest req;
            try
            {
                req = (HttpWebRequest)HttpWebRequest.Create(url);
            }
            catch (Exception err)
            {
                MessageBox.Show("Error: " + err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //TxtUrl.Focus();
                return;
            }

            //Get the WebResponse class
            HttpWebResponse resp;
            try
            {
                resp = (HttpWebResponse)req.GetResponse();
            }
            catch (WebException err)
            {
                MessageBox.Show(err.Status + " - " + err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                resp = (HttpWebResponse)err.Response;
                if (resp == null)
                {
                    //TxtUrl.Focus();
                    return;
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("Error: " + err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //TxtUrl.Focus();
                return;
            }

            //Get stream and place into temp file
            Stream rcvStream = resp.GetResponseStream();

            //Create temp USA Today internet file
            CreateTextFile(TmpFilename, rcvStream);

            resp.Close();
        }
        public static void DownloadWebpage(string url, string new_path_filename)
        {
            //Create Temp File path and name Saves to current system's temp folder
            TmpFilename = new_path_filename;
            //MessageBox.Show(TmpFilename);

            //Get the url and make sure it is not https
            if (url.StartsWith("https://"))
            {
                MessageBox.Show("https not supported");
            }
            else
            {

                //Create the HTTP Connection
                HttpWebRequest req;
                try
                {
                    req = (HttpWebRequest)HttpWebRequest.Create(url);
                }
                catch (Exception err)
                {
                    MessageBox.Show("Error: " + err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //TxtUrl.Focus();
                    return;
                }

                //Get the WebResponse class
                HttpWebResponse resp;
                try
                {
                    resp = (HttpWebResponse)req.GetResponse();
                }
                catch (WebException err)
                {
                    MessageBox.Show(err.Status + " - " + err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    resp = (HttpWebResponse)err.Response;
                    if (resp == null)
                    {
                        //TxtUrl.Focus();
                        return;
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show("Error: " + err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //TxtUrl.Focus();
                    return;
                }

                //Get stream and place into temp file
                Stream rcvStream = resp.GetResponseStream();

                //Create temp USA Today internet file
                CreateTextFile(TmpFilename, rcvStream);

                resp.Close();
            }
        }

        /// <summary>
        /// Download any object
        /// </summary>
        /// <param name="url"></param>
        /// <param name="new_path_filename"></param>
        /// <returns></returns>
        public static void Download(string url, string new_path_filename)
        {
            System.Net.WebClient client = new WebClient();

            try
            {
                //Get the url and make sure it is not https
                if (url.StartsWith("https://"))
                {
                    MessageBox.Show("https not supported");
                }
                else
                {

                    //Create the HTTP Connection
                    HttpWebRequest req;
                    try
                    {
                        req = (HttpWebRequest)HttpWebRequest.Create(url);
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show("Error: " + err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //TxtUrl.Focus();
                        return;
                    }

                    //Get the WebResponse class
                    HttpWebResponse resp;
                    try
                    {
                        resp = (HttpWebResponse)req.GetResponse();
                    }
                    catch (WebException err)
                    {
                        MessageBox.Show(err.Status + " - " + err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        resp = (HttpWebResponse)err.Response;
                        if (resp == null)
                        {
                            //TxtUrl.Focus();
                            return;
                        }
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show("Error: " + err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //TxtUrl.Focus();
                        return;
                    }

                    //Get stream and place into temp file
                    Stream rcvStream = resp.GetResponseStream();

                    //File.Create(new_path_filename).Read(rcvStream.c

                    resp.Close();
                    // Add a user agent header in case the 
                    // requested URI contains a query.
                    //client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                    //client.DownloadFile(url, new_path_filename);
                    //client.Dispose();
                    /*
                    Stream data = client.OpenRead(args[0]);
                    StreamReader reader = new StreamReader(data);
                    string s = reader.ReadToEnd();
                    Console.WriteLine(s);
                    data.Close();
                    reader.Close();*/
                }
            }
            catch (WebException err)
            {
                MessageBox.Show(err.Status + " - " + err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



            
        }

        /// <summary>
        /// GetFileNameFromFullPath
        /// </summary>
        /// <param name="fullpath"></param>
        /// <returns>myfilename.xml</returns>
        public static string GetFileNameFromFullPath(string fullpath)
        {
            int afterbackslash = fullpath.LastIndexOf(@"\") + 1;
            string filename = fullpath.Substring(afterbackslash, fullpath.Length - afterbackslash);

            return filename;
        }
    
    }
    /*
    public class Sound
    {
        private byte[] m_soundBytes;
        private string m_fileName;

        private enum Flags
        {
            /// <summary>play synchronously (default)</summary>
            SND_SYNC = 0x0000,
            /// <summary>play asynchronously</summary>
            SND_ASYNC = 0x0001,
            /// <summary>silence (!default) if sound not found</summary>
            SND_NODEFAULT = 0x0002,
            /// <summary>pszSound points to a memory file</summary>
            SND_MEMORY = 0x0004,
            /// <summary>loop the sound until next sndPlaySound</summary>
            SND_LOOP = 0x0008,
            /// <summary>don't stop any currently playing sound</summary>
            SND_NOSTOP = 0x0010,
            /// <summary>Stop Playing Wave</summary>
            SND_PURGE = 0x40,
            /// <summary>don't wait if the driver is busy</summary>
            SND_NOWAIT = 0x00002000,
            /// <summary>name is a registry alias</summary>
            SND_ALIAS = 0x00010000,
            /// <summary>alias is a predefined id</summary>
            SND_ALIAS_ID = 0x00110000,
            /// <summary>name is file name</summary>
            SND_FILENAME = 0x00020000,
            /// <summary>name is resource name or atom</summary>
            SND_RESOURCE = 0x00040004
        }

        [DllImport("winmm.dll", SetLastError = true)]
        static extern bool PlaySound(string pszSound, UIntPtr hmod, uint fdwSound);

        [DllImport("winmm.dll", SetLastError = true)]
        static extern bool PlaySoundBytes(byte[] pszSound, IntPtr hmod, Flags fdwSound);

        /// <summary>
        /// Construct the Sound object to play sound data from the specified file.
        /// </summary>
        public Sound(string fileName)
        {
            m_fileName = fileName;
        }

        /// <summary>
        /// Construct the Sound object to play sound data from the specified stream.
        /// </summary>
        public Sound(Stream stream)
        {
            // read the data from the stream
            m_soundBytes = new byte[stream.Length];
            stream.Read(m_soundBytes, 0, (int)stream.Length);
        }

        /// <summary>
        /// Play the sound
        /// </summary>
        public void Play()
        {
            // if a file name has been registered, call WCE_PlaySound,
            //  otherwise call WCE_PlaySoundBytes
            if (m_fileName != null)
                PlaySound(m_fileName, IntPtr.Zero, (int)(Flags.SND_ASYNC | Flags.SND_FILENAME));
            else
                PlaySoundBytes(m_soundBytes, IntPtr.Zero, (int)(Flags.SND_ASYNC | Flags.SND_MEMORY));
        }
    }


    */

}
