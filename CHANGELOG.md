# Changelog

All notable changes to **Xal** will be documented here.  
This changelog starts fresh from version **2.0.0**, ignoring prior releases.

---

## [2.0.0] - 2026-09-14

### 🚀 Major Changes
- Complete redesign of the library with **extension blocks** (C# 14).
- Unified naming and structure across all extension members.
- Dropped legacy APIs from 1.x in favor of a cleaner, more consistent API surface.

### ✨ New Features
- **String extensions**: parsing to nullable types, email/URL validation, diacritic removal, token replacement, truncation, title casing.
- **Date & time extensions**: start/end of day, week, month, quarter, year; difference calculations; weekend detection.
- **Collection & dictionary extensions**: chunking, splitting, index finding, conditional removal, safe value usage.
- **Numeric extensions**: clamping, range checks, rounding, truncation, absolute values, sign detection.
- **XML extensions**: namespace-agnostic element and attribute queries.
- **Encoding extensions**: Base64Url encode/decode.
- **Stream & builder extensions**: read all lines (sync/async), formatted `AppendLine`.

### 🛠️ Breaking Changes
- Removed all previous APIs from 1.x versions.  
- Replaced `extension methods` with **extension members** using C# 14 syntax.  
- Namespaces consolidated under `Xal`.

### 📦 Migration Notes
- Update your project to C# 14 or later.  
- Replace old `Xal.Extensions.*` references with the new unified `Xal` namespace.  
- Review removed methods and adapt to the new extension members.