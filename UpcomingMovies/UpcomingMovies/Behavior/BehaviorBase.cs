using System;
using Xamarin.Forms;

namespace UpcomingMovies.Behavior
{
    public class BehaviorBase<TView> : Behavior<TView> where TView : BindableObject
    {
        public TView AssociatedObject { get; private set; }
        protected override void OnAttachedTo(TView bindable)
        {
            base.OnAttachedTo(bindable);
            AssociatedObject = bindable;
            if (bindable.BindingContext != null)
            {
                BindingContext = bindable.BindingContext;
            }
            bindable.BindingContextChanged += OnBindingContextChanged;
        }
        protected override void OnDetachingFrom(TView bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.BindingContextChanged -= OnBindingContextChanged;
            AssociatedObject = null;
        }
        protected void OnBindingContextChanged(object sender, EventArgs e)
        {
            OnBindingContextChanged();
        }
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            BindingContext = AssociatedObject.BindingContext;
        }
    }
}
