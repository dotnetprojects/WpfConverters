using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Interactivity;

namespace DotNetProjects.WPF.Converters.Behaviors
{
    
    public class StylizedBehaviorCollection : Collection<Behavior>
    { }

    /// <summary>
    /// Helper Class to apply Behaviors via a Style.
    /// But the Behavior has to implement ICloneableBehavior!
    /// </summary>
    public class StylizedBehaviors
    {
        #region Fields (public)
        public static readonly DependencyProperty BehaviorsProperty = DependencyProperty.RegisterAttached(
            @"Behaviors",
            typeof(StylizedBehaviorCollection),
            typeof(StylizedBehaviors),
            new PropertyMetadata(null, OnPropertyChanged));
        #endregion
        #region Static Methods (public)
        public static StylizedBehaviorCollection GetBehaviors(DependencyObject uie)
        {
            return (StylizedBehaviorCollection)uie.GetValue(BehaviorsProperty);
        }

        public static void SetBehaviors(DependencyObject uie, StylizedBehaviorCollection value)
        {
            uie.SetValue(BehaviorsProperty, value);
        }
        #endregion

        #region Static Methods (private)
        private static void OnPropertyChanged(DependencyObject dpo, DependencyPropertyChangedEventArgs e)
        {
            var uie = dpo as UIElement;

            if (uie == null)
            {
                return;
            }

            BehaviorCollection itemBehaviors = Interaction.GetBehaviors(uie);

            var newBehaviors = e.NewValue as StylizedBehaviorCollection;
            var oldBehaviors = e.OldValue as StylizedBehaviorCollection;

            if (newBehaviors == oldBehaviors)
            {
                return;
            }

            if (oldBehaviors != null)
            {
                foreach (var behavior in oldBehaviors)
                {
                    int index = itemBehaviors.IndexOf(behavior);

                    if (index >= 0)
                    {
                        itemBehaviors.RemoveAt(index);
                    }
                }
            }

            if (newBehaviors != null)
            {
                foreach (var behavior in newBehaviors)
                {
                    int index = itemBehaviors.IndexOf(behavior);

                    if (index < 0)
                    {
                        if (behavior is ICloneableBehavior)
                        {
                            itemBehaviors.Add((Behavior) ((ICloneableBehavior) behavior).CloneBehavior());
                        }
                        else
                        {
                            throw new NotImplementedException("Behavior does not implement ICloneableBehavior");
                        }
                    }
                }
            }
        }
        #endregion
    }
}
