// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Components;

internal static class NavigationManagerExtensions
{
    public static void GoToRoot(this NavigationManager Navigator) => Navigator.NavigateTo("/");
}
