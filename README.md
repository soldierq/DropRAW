# DropRAW

Most high end DSLR can write photos captured into 2 cards (E.g., 1 CF card for RAW, 1 SD card for small JPG for quick sharing). 
When you delete the photo on SD card from camera's menu, it doesn't delete the same photo on CF card.

So this small app is designed to delete those photos on CF card which have been already deleted from SD card.

## Example:

    dotnet dropRAW.dll -b f:\\jpg -t g:\\raw

## Usage: dotnet dropRAW.dll [options]
    Options:
    -?|-h|--help                 Show help information
    -b|--base <BASE_FOLDER>      The base folder as a reference
    -t|--target <TARGET_FOLDER>  The target folder from where the files will be removed

