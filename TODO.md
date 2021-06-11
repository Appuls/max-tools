# TODO

- Change loading behavior such that the bootstrap script loads adjacent assemblies:
  ```maxScript
  pathConfig.removePathLeaf (getSourceFileName())
  ```
- Export Selection to CSV
- Finesse UI

```
EXPORT TO CSV

(disabled when selection is empty)

# items selected   [Export] <-- exports current selection to CSV with user data

ex:
Node Name | (user data key 0) | (user data key 1) | <-- user data keys are sorted alphabetically
```
