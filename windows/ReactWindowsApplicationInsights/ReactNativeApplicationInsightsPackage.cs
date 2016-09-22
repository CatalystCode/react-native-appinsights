using System;
using System.Collections.Generic;
using ReactNative.Bridge;
using ReactNative.Modules.Core;
using ReactNative.UIManager;

namespace ReactWindowsApplicationInsights
{
    public class ReactNativeApplicationInsightsPackage : IReactPackage
    {
        public IReadOnlyList<Type> CreateJavaScriptModulesConfig()
        {
            return new List<Type>(0);
        }

        public IReadOnlyList<INativeModule> CreateNativeModules(ReactContext reactContext)
        {
            return new List<INativeModule>
            {
                new ReactNativeApplicationInsightsModule(),
            };
        }

        public IReadOnlyList<IViewManager> CreateViewManagers(ReactContext reactContext)
        {
            return new List<IViewManager>(0);
        }
    }
}
