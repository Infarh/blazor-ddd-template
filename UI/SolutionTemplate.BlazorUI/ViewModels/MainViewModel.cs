using Microsoft.AspNetCore.Components;
using SolutionTemplate.BlazorUI.ViewModels.Base;

namespace SolutionTemplate.BlazorUI.ViewModels;

public class MainViewModel : TitledViewModel
{
    public NavigationManager Navigator { get; }
    public HttpClient Http { get; }

    public MainViewModel(NavigationManager Navigator, HttpClient Http)
    {
        this.Navigator = Navigator;
        this.Http = Http;
    }
}
