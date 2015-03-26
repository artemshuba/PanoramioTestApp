using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;
using Microsoft.Xaml.Interactivity;

namespace PanoramioTestApp.Behaviors
{
    public class VisibilityTransitionBehavior : DependencyObject, IBehavior
    {
        public DependencyObject AssociatedObject { get; private set; }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(Visibility), typeof(VisibilityTransitionBehavior), new PropertyMetadata(default(Visibility), PropertyChangedCallback));

        public Visibility Value
        {
            get { return (Visibility)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var b = (VisibilityTransitionBehavior)d;

            b.TransitionOut((Visibility)e.OldValue);
        }


        public static readonly DependencyProperty AnimationOutProperty =
            DependencyProperty.Register("AnimationOut", typeof(Storyboard), typeof(VisibilityTransitionBehavior), new PropertyMetadata(default(Storyboard)));

        public Storyboard AnimationOut
        {
            get { return (Storyboard)GetValue(AnimationOutProperty); }
            set { SetValue(AnimationOutProperty, value); }
        }

        public static readonly DependencyProperty AnimationInProperty =
            DependencyProperty.Register("AnimationIn", typeof(Storyboard), typeof(VisibilityTransitionBehavior), new PropertyMetadata(default(Storyboard)));

        public Storyboard AnimationIn
        {
            get { return (Storyboard)GetValue(AnimationInProperty); }
            set { SetValue(AnimationInProperty, value); }
        }

        public void Attach(DependencyObject associatedObject)
        {
            AssociatedObject = associatedObject;

            ((FrameworkElement)AssociatedObject).Visibility = Value;
        }

        public void Detach()
        {
            AssociatedObject = null;
        }

        private void TransitionOut(Visibility oldValue)
        {
            if (AssociatedObject == null)
                return;

            if (AnimationOut == null || oldValue == Visibility.Collapsed)
            {
                TransitionIn();
            }
            else
            {
                AnimationOut.Stop();
                Storyboard.SetTarget(AnimationOut, AssociatedObject);
                AnimationOut.Completed += AnimationOutCompleted;
                AnimationOut.Begin();
            }
        }

        private void TransitionIn()
        {
            if (AssociatedObject == null)
                return;

            ((FrameworkElement)AssociatedObject).Visibility = Value;
            if (AnimationIn != null)
            {
                AnimationIn.Stop();

                Storyboard.SetTarget(AnimationIn, AssociatedObject);
                AnimationIn.Begin();

            }
        }
        void AnimationOutCompleted(object sender, object e)
        {
            AnimationOut.Completed -= AnimationOutCompleted;
            TransitionIn();
        }
    }
}
