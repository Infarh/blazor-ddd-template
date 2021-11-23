namespace SolutionTemplate.BlazorUI.ViewModels.Base;

public abstract class TitledViewModel : ViewModel
{
    #region Title : string - Текст в заголовке страницы

    /// <summary>Текст в заголовке страницы</summary>
    private string _Title = "SolutionTemplate";

    /// <summary>Текст в заголовке страницы</summary>
    public string Title { get => _Title; set => Set(ref _Title, value); }

    #endregion
}
