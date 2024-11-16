using System.IO.Compression;
using System.Text;

namespace DemoCompressString
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public static string Compress(string sText)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(sText);
            var ms = new MemoryStream();
            using (var zip = new GZipStream(ms, CompressionMode.Compress, true))
            {
                zip.Write(buffer, 0, buffer.Length);
            }
            ms.Position = 0;
            var outStream = new MemoryStream();
            var compressed = new byte[ms.Length];
            ms.Read(compressed, 0, compressed.Length);
            var gzBuffer = new byte[compressed.Length + 4];
            Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4);
            return Convert.ToBase64String(gzBuffer);
        }
        public static string Decompress(string sCompressedText)
        {
            byte[] gzBuffer = Convert.FromBase64String(sCompressedText);
            using (var ms = new MemoryStream())
            {
                int msgLength = BitConverter.ToInt32(gzBuffer, 0);
                ms.Write(gzBuffer, 4, gzBuffer.Length - 4);
                ms.Position = 0;
                using (var msTemp = new MemoryStream())
                {
                    using (var zip = new GZipStream(ms, CompressionMode.Decompress))
                    {
                        zip.CopyTo(msTemp);
                        msTemp.Position = 0;
                        using (var sr = new StreamReader(msTemp))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }
        private void Decompress_Click(object sender, EventArgs e)
        {
            var resultDecompress = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    return;
                }
                resultDecompress = Decompress(textBox1.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                textBox2.Text = resultDecompress;
            }
        }

        private void Compress_Click(object sender, EventArgs e)
        {
            var resultCompress = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    return;
                }
                resultCompress = Compress(textBox1.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                textBox2.Text = resultCompress;
            }
        }
    }
}
