using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace SolutionTemplate.BlazorUI.Infrastructure.Commands.Base;

public abstract class Command : ICommand, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = null!) => PropertyChanged?.Invoke(this, new(PropertyName));

    protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string PropertyName = null!)
    {
        if (Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(PropertyName);
        return true;
    }

    public event EventHandler? CanExecuteChanged;

    protected virtual void OnCanExecuteChanged(EventArgs e) => CanExecuteChanged?.Invoke(this, e);

    private bool _Enable;

    public bool Enable
    {
        get => _Enable;
        set
        {
            if (Set(ref _Enable, value))
                OnCanExecuteChanged(EventArgs.Empty);
        }
    }

    bool ICommand.CanExecute(object? parameter) => CanExecute(parameter);

    void ICommand.Execute(object? parameter)
    {
        if (!((ICommand)this).CanExecute(parameter)) return;
        Execute(parameter);
    }

    protected virtual bool CanExecute(object? parameter) => _Enable;

    protected abstract void Execute(object? parameter);
}