

using Shop.Controls;
using Shop.Platforms.Android.Services;

using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Controls.Compatibility;

using System.ComponentModel;

using Android.Content;
using Android.Runtime;
using android = Android.Graphics;


[assembly: ExportRenderer(typeof(RangeSlider), typeof(RangeSlider_Renderer))]

namespace Shop.Platforms.Android.Services
{
    [Preserve(AllMembers = true)]
    internal class RangeSlider_Renderer : ViewRenderer<RangeSlider, RangeSliderRender>
    {
        public RangeSlider_Renderer(Context context):base(context)
        {
                
        }

        private bool _gestureEnabledPreviousState;

        protected override void OnElementChanged(ElementChangedEventArgs<RangeSlider> e)
        {
            base.OnElementChanged(e);
            if (Element == null)
                return;
            if (Control == null)
            {
                var rangeSeekBar = new RangeSliderRender(Context)
                {
                    NotifyWhileDragging = true,
                    TextAboveThumbsColor = android.Color.Black
                };
                rangeSeekBar.LowerValueChanged += RangeSeekBarLowerValueChanged;
                rangeSeekBar.UpperValueChanged += RangeSeekBarUpperValueChanged;
                rangeSeekBar.DragStarted += RangeSeekBarDragStarted;
                rangeSeekBar.DragCompleted += RangeSeekBarDragCompleted;
                SetNativeControl(rangeSeekBar);
            }
            UpdateControl(Control, Element);
        }

        private void RangeSeekBarDragCompleted(object sender, EventArgs e)
        {
            RestoreGestures();
            Element.OnDragCompleted();
        }

        private void RangeSeekBarDragStarted(object sender, EventArgs e)
        {
            Element.OnDragStarted();
            DisableGestures();
        }

        private void UpdateControl(RangeSliderRender control, RangeSlider element)
        {
            control.SetRangeValues(element.MinimumValue, element.MaximumValue);
            control.SetSelectedMinValue(element.LowerValue);
            control.SetSelectedMaxValue(element.UpperValue);
            control.MinThumbHidden = element.MinThumbHidden;
            control.MinThumbTextHidden = element.MinThumbTextHidden;
            control.MaxThumbHidden = element.MaxThumbHidden;
            control.MaxThumbTextHidden = element.MaxThumbTextHidden;
            control.StepValue = element.StepValue;
            control.StepValueContinuously = element.StepValueContinuously;
            if (element.BarHeight.HasValue)
                control.SetBarHeight(element.BarHeight.Value);
            control.ShowTextAboveThumbs = element.ShowTextAboveThumbs;
            control.TextSizeInSp = (int)element.TextSize;
            control.TextFormat = element.TextFormat;
            if (element.TextColor != Microsoft.Maui.Graphics.Color.Parse("#000000"))
                control.TextAboveThumbsColor = element.TextColor.ToAndroid();
            control.FormatLabel = element.FormatLabel;
            control.ActivateOnDefaultValues = true;
            if (element.ActiveColor != Microsoft.Maui.Graphics.Color.Parse("#000000"))
                control.ActiveColor = element.ActiveColor.ToAndroid();
            control.MaterialUI = element.MaterialUI;
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == RangeSlider.LowerValueProperty.PropertyName)
            {
                Control.SetSelectedMinValue(Element.LowerValue);
            }
            else if (e.PropertyName == RangeSlider.UpperValueProperty.PropertyName)
            {
                Control.SetSelectedMaxValue(Element.UpperValue);
            }
            else if (e.PropertyName == RangeSlider.MinimumValueProperty.PropertyName ||
                     e.PropertyName == RangeSlider.MaximumValueProperty.PropertyName)
            {
                Control.SetRangeValues(Element.MinimumValue, Element.MaximumValue);
            }
            else if (e.PropertyName == RangeSlider.MaxThumbHiddenProperty.PropertyName)
            {
                Control.MaxThumbHidden = Element.MaxThumbHidden;
            }
            else if (e.PropertyName == RangeSlider.MinThumbHiddenProperty.PropertyName)
            {
                Control.MinThumbHidden = Element.MinThumbHidden;
            }
            else if (e.PropertyName == RangeSlider.StepValueProperty.PropertyName)
            {
                Control.StepValue = Element.StepValue;
            }
            else if (e.PropertyName == RangeSlider.StepValueContinuouslyProperty.PropertyName)
            {
                Control.StepValueContinuously = Element.StepValueContinuously;
            }
            else if (e.PropertyName == RangeSlider.BarHeightProperty.PropertyName)
            {
                if (Element.BarHeight.HasValue)
                    Control.SetBarHeight(Element.BarHeight.Value);
            }
            else if (e.PropertyName == RangeSlider.ShowTextAboveThumbsProperty.PropertyName)
            {
                Control.ShowTextAboveThumbs = Element.ShowTextAboveThumbs;
                ForceFormsLayout();
            }
            else if (e.PropertyName == RangeSlider.TextSizeProperty.PropertyName)
            {
                Control.TextSizeInSp = (int)Element.TextSize;
                ForceFormsLayout();
            }
            else if (e.PropertyName == RangeSlider.TextFormatProperty.PropertyName)
            {
                Control.TextFormat = Element.TextFormat;
            }
            else if (e.PropertyName == RangeSlider.TextColorProperty.PropertyName)
            {
                if (Element.TextColor != Microsoft.Maui.Graphics.Color.Parse("#000000"))
                    Control.TextAboveThumbsColor = Element.TextColor.ToAndroid();
            }
            else if (e.PropertyName == RangeSlider.FormatLabelProperty.PropertyName)
            {
                Control.FormatLabel = Element.FormatLabel;
            }
            else if (e.PropertyName == RangeSlider.ActiveColorProperty.PropertyName)
            {
                if (Element.ActiveColor != Microsoft.Maui.Graphics.Color.Parse("#000000"))
                    Control.ActiveColor = Element.ActiveColor.ToAndroid();
            }
            else if (e.PropertyName == RangeSlider.MaterialUiProperty.PropertyName)
            {
                Control.MaterialUI = Element.MaterialUI;
            }
            else if (e.PropertyName == RangeSlider.MinThumbTextHiddenProperty.PropertyName)
            {
                Control.MinThumbTextHidden = Element.MinThumbTextHidden;
            }
            else if (e.PropertyName == RangeSlider.MaxThumbTextHiddenProperty.PropertyName)
            {
                Control.MaxThumbTextHidden = Element.MaxThumbTextHidden;
            }
        }

        private void ForceFormsLayout()
        {
            //HACK to force Xamarin.Forms layout engine to update control size
            if (!Element.IsVisible) return;
            Element.IsVisible = false;
            Element.IsVisible = true;
        }

        private void RangeSeekBarUpperValueChanged(object sender, EventArgs e)
        {
            Element.OnUpperValueChanged(Control.GetSelectedMaxValue());
        }

        private void RangeSeekBarLowerValueChanged(object sender, EventArgs e)
        {
            Element.OnLowerValueChanged(Control.GetSelectedMinValue());
        }

        // TODO find less weird hack to make slider work on Master-Detail page
        private void DisableGestures()
        {
            var masterDetailPage = Application.Current.MainPage as FlyoutPage;
            if (masterDetailPage != null)
            {
                _gestureEnabledPreviousState = masterDetailPage.IsGestureEnabled;
                masterDetailPage.IsGestureEnabled = false;
            }
        }

        private void RestoreGestures()
        {
            var masterDetailPage = Application.Current.MainPage as FlyoutPage;
            if (masterDetailPage != null)
            {
                masterDetailPage.IsGestureEnabled = _gestureEnabledPreviousState;
            }
        }

    }
}
