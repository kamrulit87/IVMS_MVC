using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace IVMS.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Content/built_in/assets/plugins/modernizr.custom.js",
                "~/Content/built_in/assets/plugins/jquery-ui/jquery-ui.min.js",
                "~/Content/built_in/assets/plugins/boostrapv3/js/bootstrap.min.js",
                "~/Content/built_in/assets/plugins/jquery-scrollbar/jquery.scrollbar.min.js",
                "~/Content/built_in/assets/plugins/jquery-validation/js/jquery.validate.min.js",
                "~/Content/built_in/assets/js/form_layouts.js",
                "~/Content/built_in/assets/js/form_elements.js"
            ));


            bundles.Add(new StyleBundle("~/bundles/css").Include(
                "~/Content/built_in/assets/plugins/boostrapv3/css/bootstrap.min.css",
                "~/Content/built_in/assets/plugins/jquery-scrollbar/jquery.scrollbar.css"
            ));



            BundleTable.EnableOptimizations = true;

        }
    }
}