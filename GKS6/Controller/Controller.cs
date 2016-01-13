namespace GKS6.Controller
{
    class Controller
    {
        private MainWindow mainWindow;
        private Page0 page0;
        private Page1 page1;
        private Page2 page2;

        public Controller(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;

            page0 = new Page0();

            page0.MainDataCreated += page0_MainDataCreated;
            page0.MainDataLoaded += page0_MainDataLoaded;

            mainWindow.frameMain.Content = page0;

            mainWindow.buttonPage1.IsEnabled = false;
            mainWindow.buttonPage2.IsEnabled = false;
        }



        internal void GoToPage0()
        {
            mainWindow.frameMain.Content = page0;
        }

        internal void GoToPage1()
        {
            mainWindow.frameMain.Content = page1;
        }

        internal void GoToPage2()
        {
            mainWindow.frameMain.Content = page2;
        }


        void page0_MainDataCreated(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            page1 = new Page1(page0.MainData);

            page2 = new Page2(page0.MainData);
        }

        void page0_MainDataLoaded(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            mainWindow.buttonPage1.IsEnabled = true;
            mainWindow.buttonPage2.IsEnabled = true;
        }

    }
}



