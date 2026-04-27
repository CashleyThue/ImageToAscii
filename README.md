# Image To ASCII

Converts images into ASCII art using ImageSharp.


## Build From Source

### Prerequisites
Requires .NET 10 SDK (or use published standalone binaries available in Releases)

Clone the repository:

```bash
git clone https://github.com/yourusername/ImageToAscii.git
cd ImageToAscii
```

Restore dependencies:

```bash
dotnet restore
```

Build and run:

```bash
dotnet run -- inputImg scale outputTxt
```

---

## Publish Standalone Executable

### Linux

```bash
dotnet publish -c Release \
-r linux-x64 \
--self-contained true \
-p:PublishSingleFile=true
```

Output is stored at:

```text
bin/Release/net10.0/linux-x64/publish/ImageToAscii
```

---

### Windows

```powershell
dotnet publish -c Release `
-r win-x64 `
--self-contained true `
-p:PublishSingleFile=true
```

Output is stored at:

```text
bin\Release\net10.0\win-x64\publish\ImageToAscii.exe
```

---

### macOS Intel

```bash
dotnet publish -c Release \
-r osx-x64 \
--self-contained true \
-p:PublishSingleFile=true
```

---

### macOS Apple Silicon

```bash
dotnet publish -c Release \
-r osx-arm64 \
--self-contained true \
-p:PublishSingleFile=true
```

After publishing, move the executable into your PATH if desired.
## Usage
ImageToAscii [input] [scale] [output]

Example:
```bash
ImageToAscii cat.jpg 5 cat.txt
```
