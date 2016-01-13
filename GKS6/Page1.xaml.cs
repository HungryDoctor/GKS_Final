using GKS.Additions;
using GKS.GKS.Additions;
using QuickGraph;
using System.Windows.Controls;

namespace GKS6
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        private MainData data;

        public Page1(MainData mainData)
        {
            InitializeComponent();

            this.data = mainData;
            MakeGridsReadOnly();
            SetMaxSizes();
            SetValues();
        }



        private void SetValues()
        {
            Dispatcher.Invoke(() => Common.AddListOfListsToDataGrid(DataGrid_Initial, data.InitialList));
            Dispatcher.Invoke(() => Common.AddListOfListsToDataGrid(DataGrid_Comparation, data.ComparationList));
            Dispatcher.Invoke(() => Common.SetDictionaryToListView(ListView_GroupsUnsimplified, data.GropsFull));
            Dispatcher.Invoke(() => Common.SetDictionaryToListView(ListView_GroupsSimplified, data.GroupsSimplified));
        }

        private void ClearGraphLayout()
        {
            Common.ShowGraphCircular(GraphLayout_GroupInitial, new BidirectionalGraph<object, IEdge<object>>(), Constants.HORIZONTAL_GAP, Constants.VERTICAL_GAP);
            Common.ShowGraphCircular(GraphLayout_GroupModuled, new BidirectionalGraph<object, IEdge<object>>(), Constants.HORIZONTAL_GAP, Constants.VERTICAL_GAP);
        }

        private void UnselectAll()
        {
            ListView_GroupsSimplified.Focus();

            DataGrid_Initial.UnselectAll();
            DataGrid_Comparation.UnselectAll();

            ListView_GroupsUnsimplified.UnselectAll();
            ListView_GroupsSimplified.UnselectAll();
        }

        private void MakeGridsReadOnly()
        {
            DataGrid_Initial.IsReadOnly = true;
            DataGrid_Initial.CanUserAddRows = false;
            DataGrid_Initial.CanUserDeleteRows = false;
            DataGrid_Initial.CanUserReorderColumns = false;
            DataGrid_Initial.CanUserResizeColumns = false;
            DataGrid_Initial.CanUserResizeRows = false;
            DataGrid_Initial.CanUserSortColumns = false;
            DataGrid_Initial.SelectionMode = DataGridSelectionMode.Single;
            DataGrid_Initial.SelectionUnit = DataGridSelectionUnit.Cell;

            DataGrid_Comparation.IsReadOnly = true;
            DataGrid_Comparation.CanUserAddRows = false;
            DataGrid_Comparation.CanUserDeleteRows = false;
            DataGrid_Comparation.CanUserReorderColumns = false;
            DataGrid_Comparation.CanUserResizeColumns = false;
            DataGrid_Comparation.CanUserResizeRows = false;
            DataGrid_Comparation.CanUserSortColumns = false;
            DataGrid_Comparation.SelectionMode = DataGridSelectionMode.Single;
            DataGrid_Comparation.SelectionUnit = DataGridSelectionUnit.Cell;
        }

        private void SetMaxSizes()
        {
            DataGrid_Initial.MaxWidth = 198;
            DataGrid_Initial.MaxHeight = 384;
            DataGrid_Initial.Width = double.NaN;
            DataGrid_Initial.Height = double.NaN;

            DataGrid_Comparation.MaxWidth = 348;
            DataGrid_Comparation.MaxHeight = 384;
            DataGrid_Comparation.Width = double.NaN;
            DataGrid_Comparation.Height = double.NaN;

            ListView_GroupsUnsimplified.MaxWidth = 294;
            ListView_GroupsUnsimplified.MaxHeight = 160;
            ListView_GroupsUnsimplified.Width = double.NaN;
            ListView_GroupsUnsimplified.Height = double.NaN;

            ListView_GroupsSimplified.MaxWidth = 294;
            ListView_GroupsSimplified.MaxHeight = 160;
            ListView_GroupsSimplified.Width = double.NaN;
            ListView_GroupsSimplified.Height = double.NaN;

            GraphLayout_GroupInitial.MaxWidth = 430;
            GraphLayout_GroupInitial.MaxHeight = 249;
            GraphLayout_GroupInitial.Width = double.NaN;
            GraphLayout_GroupInitial.Height = double.NaN;

            GraphLayout_GroupModuled.MaxWidth = 430;
            GraphLayout_GroupModuled.MaxHeight = 249;
            GraphLayout_GroupModuled.Width = double.NaN;
            GraphLayout_GroupModuled.Height = double.NaN;
        }


        private void ListView_GroupsSimplified_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListView_GroupsSimplified.SelectedItem != null && ListView_GroupsSimplified.SelectedIndex != -1)
            {
                Common.ShowGraphCircular(GraphLayout_GroupInitial, data.GraphsListInitial[ListView_GroupsSimplified.SelectedIndex], Constants.HORIZONTAL_GAP, Constants.VERTICAL_GAP);
                Common.ShowGraphCircular(GraphLayout_GroupModuled, data.GraphsListModuled[ListView_GroupsSimplified.SelectedIndex], Constants.HORIZONTAL_GAP, Constants.VERTICAL_GAP);
            }
            else
            {
                ClearGraphLayout();
            }
        }

        private void DataGrid_Comparation_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex()).ToString();
        }

        private void DataGrid_Initial_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex()).ToString();
        }

        private void Page_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (gridBottom.IsMouseOver == false)
            {
                UnselectAll();
            }
        }

    }
}
