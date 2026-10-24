using Avalonia.Controls;
using SistemaInventarioComposicion.Gui.ViewModels;

namespace SistemaInventarioComposicion.Gui.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        var vm = new MainWindowViewModel();
        vm.SolicitarConfirmacion = async mensaje =>
        {
            var dlg = new ConfirmacionWindow(mensaje);
            var result = await dlg.ShowDialog<bool?>(this);
            return result == true;
        };
        DataContext = vm;
    }
}