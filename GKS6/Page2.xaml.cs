using GKS;
using GKS.Additions;
using GKS.GKS.Additions;
using QuickGraph;
using System.Linq;
using System.Windows.Controls;

namespace GKS6
{
    /// <summary>
    /// Interaction logic for Page2.xaml
    /// </summary>
    public partial class Page2 : Page
    {
        private MainData data;

        public Page2(MainData mainData)
        {
            InitializeComponent();

            this.data = mainData;
            SetMaxSizes();
            SetValues();
        }



        private void SetValues()
        {
            Dispatcher.Invoke(() => Common.SetDictionaryToListView(ListView_ModulesFull, data.ModulesFull));
            Dispatcher.Invoke(() => Common.SetStringToListView(ListView_ModulesUnique, data.ModulesUnique));

            Dispatcher.Invoke(() => Common.ShowGraphCircular(GraphLayout_Flows, data.GraphFlows, 35, 25));
        }

        private void UnselectAll()
        {
            ListView_ModulesFull.UnselectAll();
            ListView_ModulesUnique.UnselectAll();
        }

        private void SetMaxSizes()
        {
            ListView_ModulesFull.MaxWidth = 294;
            ListView_ModulesFull.MaxHeight = 160;
            ListView_ModulesFull.Width = double.NaN;
            ListView_ModulesFull.Height = double.NaN;

            ListView_ModulesUnique.MaxWidth = 294;
            ListView_ModulesUnique.MaxHeight = 160;
            ListView_ModulesUnique.Width = double.NaN;
            ListView_ModulesUnique.Height = double.NaN;
        }


        private void Page_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            UnselectAll();
        }

    }
}
