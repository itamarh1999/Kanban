using System.ComponentModel;

namespace Frontend;

public class NotifiableObject : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    protected void RaisePropertyChanged(string property)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs
            (property));
    }
}