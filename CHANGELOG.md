# Changelog

Significant changes in this project will be documented here.

### [1.0.5] - 2018-10-20

### Changed

The signatures of the `BeforeApply` and `AfterApply` events of `DateTimeSpecification` has been changed.

```C#
// Before:
EventHandler(object sender, ref DateTime date)

// Now:
EventHandler(object sender, DateTimeEventArgs e)
```

### Added

- The new class `DateTimeEventArgs` is now used to provide data for the events of the `DateTimeSpecification` class.
- Strings has now `.AsBoolean()` and `.ToBoolean` extension methods.