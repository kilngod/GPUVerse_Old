using GPUMauiApp.Pages;
using GPUMauiLib;

namespace GPUMauiApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new GPUViewPage();
    }


    protected override Window CreateWindow(IActivationState activationState)
    {

        GPUWindow window = null;
        if (Windows.Count > 0)
            return Windows[0];


        if (this.MainPage != null)
        {
            window = new GPUWindow(MainPage);
        }
        else
        {
            window = new GPUWindow();
        }
#if WINDOWS
            window.App = this;
#endif

        return window;
    }
}