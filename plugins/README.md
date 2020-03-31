# Plugins (Unofficial)
```diff
- IMPORTANT: Please note that any plugins are not officially supported and are only there for demonstration purposes
```

### nopCommerce

Instructions for demo purposes:

- Clone [nopCommerce](https://github.com/nopSolutions/nopCommerce) and checkout `master`
- Install [Microsoft SQL Service Compact](https://www.microsoft.com/en-us/download/details.aspx?id=17876) (when you do not have SQL Server)
- Copy `nopCommerce/Nop.Plugin.Payments.RaboOmniKassa` project into `/src/Plugins` of nopCommerce and add it to the solution.
- Add a proper OmniKassa reference to the `Nop.Plugin.Payments.RaboOmniKassa` project.
- As `Nop.Core` is using an older version of Newtonsoft.Json, re-add a reference to version 11.0.2 to prevent any conflicts.
- Make sure all projects are successfully build.
- Launch `Nop.Web` to start the installation (database configuration etc.)
- Check `Create sample data` (if you want sample data) and complete the configuration
- Go to the Administration panel -> Configuration -> Plugins -> Local plugins, search for `Rabo OmniKassa` and press install
- Go to Configuration -> Payment -> Payment methods, press Configure and enter a Signing key and Token. Make sure `Use Sandbox` is selected.
- Go back, press Edit and set the plugin active.

Rabo OmniKassa should now be available on checkout.