namespace VersionControl_v3 {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        bool canClickStartButton = true;

        private void fake_Transport() {
            Random random = new Random();
            int r = random.Next(20);
            Thread.Sleep(r);
            Console.WriteLine("sleep time:" + r.ToString());
        }

        private void StartSearchAndTransport(object sender, EventArgs e) {

            if (canClickStartButton == false) { return; }

            string fromPath = this.fromPathTextBox.Text;
            string toPath = this.toPathTextBox.Text;

            if (toPath == fromPath || Directory.Exists(fromPath) == false || Directory.Exists(toPath) == false) {
                MessageBox.Show("Invaild file path");
                return;
            }

            canClickStartButton = false;
            StartButton.Text = "Transporting...";
            TotalFileTransportCountLabel.Text = "Total files transport count:" + VersionControl.GetAllFileCount(fromPath).ToString();

            if (this.doCopyWholeFolder.Checked) {
                VersionControl.TransportAll(fromPath, toPath+"\\");
            } else {
                VersionControl.SearchAndTransport(fromPath, toPath);
            }
            Thread.Sleep(4000);

            StartButton.Text = "Next Transport";
            TotalFileTransportCountLabel.Text = "Total files transport count:";
            doCopyWholeFolder.CheckState = CheckState.Unchecked;
            canClickStartButton = false;


        }
    }
}