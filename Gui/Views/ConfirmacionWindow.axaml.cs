using Avalonia.Controls;
using Avalonia.Interactivity;

namespace SistemaInventarioComposicion.Gui.Views;

public partial class ConfirmacionWindow : Window
{
    public bool Resultado { get; private set; }

    public ConfirmacionWindow(string mensaje)
    {
        InitializeComponent();
        MensajeTexto.Text = mensaje;
    }

    private void Aceptar_Click(object? sender, RoutedEventArgs e)
    {
        Resultado = true;
        Close(true);
    }

    private void Cancelar_Click(object? sender, RoutedEventArgs e)
    {
        Resultado = false;
        Close(false);
    }
}
