# Chirp: Quantum Console Extensions
This package contains several standalone extensions to the [Quantum Console](https://assetstore.unity.com/packages/tools/utilities/quantum-console-128881) asset store package along with [Chirp Logging Framework](https://github.com/JakubSlaby/Chirp) integration.
### Standalone Features
- Collapsing repeated log messages
- Search
- Selecting logs and previewing Stack Traces

### Chirp Features
- Channel filter


##### Compatibility
- [Quantum Console](https://assetstore.unity.com/packages/tools/utilities/quantum-console-128881) 2.4.7+
- [Chirp Logging Framework](https://github.com/JakubSlaby/Chirp) 0.8.5+
- Unity version dependant on Quantum Console and Chirp Logging Framework.

## Installation
To install Quantum Console extensions, go to [Releases](https://github.com/JakubSlaby/Chirp-QuantumConsole/releases) and download the latest `ChirpQuantumConsole_VERSION.unitypackage` and import it in your Unity project through `Assets/Import Package/Custom Package`.

After that all you need to do is to convert the default Quantum Console instance to Chirp Quantum Console instance. To do this, find your Quantum Console GameObject and right click on the QuantumConsole component - You should see a `Convert to Chirp` option. This will perform all the required settings.
Keep in mind that the conversion process will set QuantumConsole to work in SingletonMode.

#### Installation with Chirp Logging Framework
To bind Quantum Console with Chirp Logging Framework you need to import the second package `ChirpQuantumConsoleLogger_VERSION.unitypackage` which consists all the required options.

> Chirp Logging Framework requires a minimum version of Unity 2019+ - so the integration will not work on the minimum required version of Quantum Console which is Unity 2018.4.13.

After you imported the package you need to initialize the QuantumConsoleLogger.
If you're using the `Chirp Initializer` an additional option for QuantumConsoleLogger will appear automatically on the component - just enable that and follow the instructions.

If you are initialising the Chirp framework manually add QUantumConsoleLogger to the initialize method.
```csharp
Chirp.Initialize(new UnityConsoleLogger(), new QuantumConsoleLogger());
```
And run the `Configure for Chirp Logger` context menu (right click) option on QuantumConsole component - this will make sure Quantum Console is in Singleton Mode, and has disabled interception of debug messages as we handle that through Chirp.

## Using extensions
#### Collapsing repeated log messages
Collapsing will happen automatically and will group all repeating log messages together.
This functionality takes in to Stack Traces (when enabled) while comparing messages.
#### Search
To search for a specific text in logged messages just type `search SEARCH_TEXT` in the console commands.
#### Selecting messages
To select a specific message you just need to click on it in the console.
This will allow you to see the details of the message like the Stack Trace.
#### Back
Whenever you search or select the message detailed view, to go back to previous view just type in `back` command.
#### Channel filtering (Chirp only)
When using the Chirp Logging Framework you can filter by specific channel. The command field will suggest all available channels to you whenever you start tying `filter` command.
