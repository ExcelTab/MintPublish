using ImageMagick;

using System.Net;


namespace Mint.Forms
{
    public partial class ImageBox : Form
    {
        public string title;
        public List<string> imageURLs = new List<string>();

        public ImageBox()
        {
            InitializeComponent();
        }

        public async Task HideImage()
        {
            // Utilisez Invoke pour modifier les contrôles d'interface utilisateur depuis un thread différent
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    // Modifier la visibilité du PictureBox
                    this.pictureBox1.Visible = false;
                }));
            }
            else
            {
                // Vous êtes déjà dans le thread de l'interface utilisateur, donc pas besoin d'Invoke
                this.pictureBox1.Visible = false;
            }
        }

        private async void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            // ... Autres opérations

            // Utilisez Invoke pour modifier les contrôles d'interface utilisateur depuis un thread différent
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    // Modifier la visibilité du PictureBox
                    this.pictureBox1.Visible = false;
                }));
            }
            else
            {
                // Vous êtes déjà dans le thread de l'interface utilisateur, donc pas besoin d'Invoke
                this.pictureBox1.Visible = false;
            }
        }

        public async Task ShowImage(string imageURL)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    byte[] imageBytes = client.DownloadData(imageURL);

                    using (MagickImage image = new MagickImage(imageBytes))
                    {
                        // Convertir l'image ImageMagick en format PNG
                        byte[] pngBytes = image.ToByteArray(MagickFormat.Png);

                        // Utilisez Invoke pour modifier les contrôles d'interface utilisateur depuis un thread différent
                        if (this.InvokeRequired)
                        {
                            this.Invoke(new Action(() =>
                            {
                                // Charger l'image PNG dans le PictureBox
                                using (var stream = new System.IO.MemoryStream(pngBytes))
                                {
                                    this.pictureBox1.Image = System.Drawing.Image.FromStream(stream);
                                    this.pictureBox1.Visible = true;
                                }
                            }));
                        }
                        else
                        {
                            // Vous êtes déjà dans le thread de l'interface utilisateur, donc pas besoin d'Invoke
                            using (var stream = new System.IO.MemoryStream(pngBytes))
                            {
                                this.pictureBox1.Image = System.Drawing.Image.FromStream(stream);
                                this.pictureBox1.Visible = true;
                            }
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                // Gérez l'erreur ici (par exemple, affichez un message à l'utilisateur)
                //MessageBox.Show("Erreur lors du téléchargement de l'image : " + ex.Message);
            }
        }



        private void ImageBox_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Empêcher la fermeture par défaut
            e.Cancel = true;

            // Masquer le formulaire au lieu de le fermer
            this.Visible = false;
        }
    }
}
