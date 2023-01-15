

using Microsoft.Extensions.Localization;


namespace Shop.Helpers
{
    [ContentProperty(nameof(Key))]
    internal class LocalizeExtension : IMarkupExtension
    {

        readonly IStringLocalizer<Shop.Resources.Strings.Resource> _localizer;


        public string Key { get; set; } = string.Empty;

        public LocalizeExtension()
        {
            _localizer = ServiceHelper.GetService<IStringLocalizer<Shop.Resources.Strings.Resource>>();
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            string localizedText = _localizer[Key];
            return localizedText;
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);
    }
}
