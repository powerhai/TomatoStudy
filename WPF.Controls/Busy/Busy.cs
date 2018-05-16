using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;
namespace WPF.Controls.Busy
{
    public class Busy
    { 
          public static void SetShow(FrameworkElement target, bool value) {
              target.SetValue(ShowProperty, value);
          }
  
          public static bool GetShow(FrameworkElement target) {
             return (bool)target.GetValue(ShowProperty);
          }

        public static readonly DependencyProperty ShowProperty = DependencyProperty.RegisterAttached("Show", typeof(bool), typeof(Busy), new PropertyMetadata(ShowChanged));

        private static void ShowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
            Update((FrameworkElement)d);
        }
        public static readonly DependencyProperty AdornerProperty = DependencyProperty.RegisterAttached("Adorner", typeof(BusyIndicatorAdorner), typeof(Busy));

        public static void SetAdorner(FrameworkElement target, BusyIndicatorAdorner adorner)
        {
            target.SetValue(AdornerProperty, adorner);
        }

        public static BusyIndicatorAdorner GetAdorner(FrameworkElement target)
        {
            return (BusyIndicatorAdorner)target.GetValue(AdornerProperty);
        }

        private static void Update(FrameworkElement target)
        {
            var layer = AdornerLayer.GetAdornerLayer(target);
            if (layer == null)
            {
                Dispatcher.CurrentDispatcher.BeginInvoke(new Action<FrameworkElement>(o => Update(o)), DispatcherPriority.Loaded, target);
                return;
            }
             
            var show = GetShow(target);  

            var adorner = GetAdorner(target);

            if (show)
            {
                if (adorner == null)
                {
                    adorner = new BusyIndicatorAdorner(target)
                    { 
                    };
                    layer.Add(adorner);
                    SetAdorner(target, adorner);
                }
                else
                {
                    
                }
                //adorner.Visibility = Visibility.Visible;
            }
            else
            {
                if (adorner != null)
                {
                    layer.Remove(adorner);
                    //如果不 Remove 并设置为 null, 在 使用AvalonDock的程序里，切换标签会使 adorner 的 Parent 丢失
                    //如果设置为 null ，会在再一次显示的时候，重建
                    //adorner.Visibility = Visibility.Collapsed;
                    SetAdorner(target, null);
                }
            }
        }

        public class BusyIndicatorAdorner : Adorner
        {
            public UIElement mBorder;
            public BusyIndicatorAdorner(UIElement adornedElement)
                : base(adornedElement)
            {
                mBorder = new BusyBox();
                this.AddVisualChild(mBorder);
            }

            protected override Visual GetVisualChild(int index)
            {
                return mBorder;
            }
            protected override int VisualChildrenCount
            {
                get
                {
                    return 1;
                }
            }

            protected override Size ArrangeOverride(Size finalSize)
            {
                var size = base.ArrangeOverride(finalSize);
                if (mBorder != null)
                    mBorder.Arrange(new Rect(0, 0, finalSize.Width ,  finalSize.Height ));
                return size;
            }
        }

    }
}
