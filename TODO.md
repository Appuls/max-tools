# TODO

- Rename to MaxToolsApp | MaxToolsAppUI because maxscript can't load an executable as an assembly.
- Create maxscript & wpf app bootstrap
- De-risk: react to changes in the current selection in WPF
- Implement desired bulk edit logic:
```
CURRENT SELECTION PROPERTIES

[x] name   value|"varies":dropdown   [select] <-- shift-clicking adds to selection, x button removes

------------------------

ADD PROPERTY

[+] [name] [value] <-- adding overwrites existing values

------------------------

EXPORT TO CSV

# items selected   [Export] <-- exports current selection to CSV with user data

ex:
Node Name | (user data key 0) | (user data key 1) | <-- user data keys are sorted alphabetically
```
