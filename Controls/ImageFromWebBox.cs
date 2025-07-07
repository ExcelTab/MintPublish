using ImageMagick;
using System.Drawing.Imaging;
using System.Net;


namespace Mint.Controls
{
    public partial class ImageFromWebBox : UserControl
    {
        public string title;
        public string id_order_detail;
        public List<string> imageURLs = new List<string>();
        public int MaxSize = 120;

        public ImageFromWebBox()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            PictureBox pictureBox = (PictureBox)sender;
            if (pictureBox.Image != null)
            {
                // Create a new form
                Form imageForm = new Form();
                imageForm.FormBorderStyle = FormBorderStyle.FixedSingle;
                imageForm.Text = label1.Text;

                // Create a PictureBox control on the new form
                PictureBox enlargedPictureBox = new PictureBox();
                enlargedPictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
                enlargedPictureBox.Image = pictureBox.Image;

                // Set the size of the new form based on the image size

                double ratio = (double)enlargedPictureBox.Image.Height / enlargedPictureBox.Image.Width;
                //if the height of the image is bigger than the screen height, we resize the image to fit the screen
                if (enlargedPictureBox.Image.Height > Screen.PrimaryScreen.Bounds.Height)
                {
                    imageForm.Size = new Size(Convert.ToInt32((Screen.PrimaryScreen.Bounds.Height - 100) / (ratio)), Screen.PrimaryScreen.Bounds.Height - 100);
                    enlargedPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                }
                else
                {
                    imageForm.Size = new Size(enlargedPictureBox.Image.Width + 20, enlargedPictureBox.Image.Height + 20);
                }

                imageForm.StartPosition = FormStartPosition.CenterParent;
                enlargedPictureBox.Dock = DockStyle.Fill;



                // Add the PictureBox to the new form
                imageForm.Controls.Add(enlargedPictureBox);

                // Show the new form
                BlackOverlay blackOverlay = new BlackOverlay();
                blackOverlay.Show();
                imageForm.ShowDialog();
                blackOverlay.Close();
            }
        }

        public async void LoadPictures(string imageReference, string imageURL)
        {
            // refresh the images
            title = imageReference;
            imageURLs.Clear();
            imageURLs.Add(imageURL);

            label1.Text = title;

            pictureBox1.Visible = true;
            try
            {
                //Show Waiting screen
                await Task.Run(async () => { await HideImage(); });
                //Show the first image
                await Task.Run(async () => { await ShowImage(imageURLs[0]); });


            }
            catch (Exception ex)
            {
                // Handle exceptions
                MessageBox.Show($"An error occurred while showing the image: {ex.Message}");
            }



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
                                //this.pictureBox1.SizeMode = PictureBoxSizeMode.Normal;
                                using (var stream = new System.IO.MemoryStream(pngBytes))
                                {
                                    this.pictureBox1.Image = System.Drawing.Image.FromStream(stream);
                                }

                                //double ratio = (double)this.pictureBox1.Image.Height / this.pictureBox1.Image.Width;
                                ////if the height of the image is bigger than the screen height, we resize the image to fit the screen
                                //if (ratio > 1)
                                //{
                                //    //Vertical
                                //    this.Size = new Size(MaxSize * Convert.ToInt32(ratio), MaxSize);
                                //}
                                //else if (ratio < 1)
                                //{
                                //    //Horizontal
                                //    this.Size = new Size(MaxSize, MaxSize * Convert.ToInt32(ratio));
                                //}
                                //else if (ratio == 1)
                                //{
                                //    //Square
                                //    this.Size = new Size(MaxSize, MaxSize);
                                //}
                                //this.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                                this.pictureBox1.Visible = true;
                            }));
                        }
                        //else
                        //{
                        //    // Vous êtes déjà dans le thread de l'interface utilisateur, donc pas besoin d'Invoke
                        //    using (var stream = new System.IO.MemoryStream(pngBytes))
                        //    {
                        //        this.pictureBox1.Image = System.Drawing.Image.FromStream(stream);
                        //        this.pictureBox1.Visible = true;
                        //    }
                        //}

                        
                    }
                }
            }
            catch (WebException ex)
            {
                // Gérez l'erreur ici (par exemple, affichez un message à l'utilisateur)
                //MessageBox.Show("Erreur lors du téléchargement de l'image : " + ex.Message);
            }
        }
    }
}
