using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Drawing;
using System.Threading;

namespace NotificationAreaManager
{
    public class NotificationManager: INotifyPropertyChanged
    {
        private Icon _iconObject;
        public Icon IconObject
        {
            get { return _iconObject; }
            set { 
                _iconObject = value;
                OnPropertyChanged("IconObject");
            }
        }


        private ContextMenu _contextMenuObject;
        public ContextMenu ContextMenuObject { 
            get
            {
                return _contextMenuObject;
            }
            set 
            {
                _contextMenuObject = value;
                OnPropertyChanged("ContextMenuObject");
            } 
        }

        List<Action<string>> _contextHandlers;

        NotificationManagerWindow _notifyWind;

        public event PropertyChangedEventHandler PropertyChanged;

        public void Start(Icon icon, List<string> context, List<Action<string>> contextHandlers)
        {
            Thread t = new Thread(() =>
            {
                StartInSTAThread(icon, context, contextHandlers);
            });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        private void StartInSTAThread(Icon icon, List<string> context, List<Action<string>> contextHandlers) { 
            _notifyWind = new NotificationManagerWindow();
            _notifyWind.DataContext = this;
            _notifyWind.Show();

            if (icon != null)
            {
                IconObject = icon;
            }
            if(context?.Any() == true)
            {
                ContextMenuObject = new ContextMenu();
                foreach(string item in context)
                {
                    if(string.Equals(item, "separator", StringComparison.InvariantCultureIgnoreCase))
                    {
                        ContextMenuObject.Items.Add(new Separator());
                    }
                    else
                    {
                        MenuItem mi = new MenuItem();
                        mi.Header = item;
                        mi.Click += Mi_Click;
                        ContextMenuObject.Items.Add(mi);
                    }
                }
            }
            _contextHandlers = contextHandlers;
        }

        private void Mi_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            int index = 0;
            MenuItem item = sender as MenuItem;
            if(item != null)
            {
                index = ContextMenuObject.Items.IndexOf(sender);
                if (index > -1)
                {
                    if (_contextHandlers.Count > index)
                    {
                        _contextHandlers[index].Invoke(item.Header as string);
                    }
                }
            }
        }
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
