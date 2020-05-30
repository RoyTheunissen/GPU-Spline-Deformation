# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).


## [0.2.1] - 23-05-2020
### Changed  
 - Cleared selection when switching views
 - Fixed issue with missing EndHorizontal

## [0.2.0] - 23-05-2020
### Added 
 - Selection tools 
 - Batch actions
    - Select all objects on project view
    - Move selected sprites to Atlas
    - Remove sprite from atlas
 - Added option to tweak the framerate of the capture (for heavy projects)
 - Added option to manually capture frames on demand 
  
### Changed  
 - Fixed issues while loading all sprites from Atlas
 - Fixed issues where result view would not update properly after changes
 - Fixed size detection issues
 - Fixed better handling of dirty management
 - Fixed issue when tweaking allowed size variation was not been updated
 - Fixed an issue related to ReflectionTypeLoadException


## [0.1.0] - 10-05-2020

### Added 
 - Atlas View (Show all the available Atlas in the project)
 - Sprite View (Show all the available sprites in the project)
 - Added better support for finding the items
 - Added custom search box for the results
 - Added right click context menu to look for the results
 - Better filter implementation for any views
 - Added button to set sprite resolution to best use case
 
 ### Changed
 - Fixed a bunch of issues related to now loading all the sprites from the single texture
 - Update the core system to not work with a bunch of dictionary but a new SpriteData Class
 - Refactoring of multiple parts of the code
 - Multiple bug fixing related to storing / loading data

## [0.0.1]
 - Basic implementation 

## [Unreleased]
 - 

[0.2.1]: https://github.com/badawe/SpriteAuditor/releases/tag/v0.2.1
[0.2.0]: https://github.com/badawe/SpriteAuditor/releases/tag/v0.2.0
[0.1.0]: https://github.com/badawe/SpriteAuditor/releases/tag/v0.1.0
[0.0.1]: https://github.com/badawe/SpriteAuditor/releases/tag/v0.0.1


