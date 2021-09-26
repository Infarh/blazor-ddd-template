namespace SolutionTemplate.BlazorUI.Infrastructure
{
    public class MenuToggler
    {
        public string Show { get; private set; }

        public void OnChange() => Show = Show is null ? "show" : null;
    }
}
