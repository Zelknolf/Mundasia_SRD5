cd Mund_SRD5_Client\Mund_SRD5_Client
md bin
cd bin
md Debug
cd Debug
mklink /D "DataArrays" "..\..\..\..\_Release\DataArrays"
mklink /D "Images" "..\..\..\..\_Release\Images"
mklink /D "TextLibraries" "..\..\..\..\_Release\TextLibraries"
cd ..\..\..\..
cd Mund_SRD5_Server\Mund_SRD5_Server
md bin
cd bin
md Debug
cd Debug
mklink /D "DataArrays" "..\..\..\..\_Release\DataArrays"
mklink /D "Images" "..\..\..\..\_Release\Images"
mklink /D "TextLibraries" "..\..\..\..\_Release\TextLibraries"
mklink /D "Material" "..\..\..\..\_Build\Material"