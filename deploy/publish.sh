#!/usr/bin/env bash
# Yerel veya CI'da hosting'e yüklenecek publish çıktısı üretir.
set -euo pipefail
cd "$(dirname "$0")/.."
export PATH="${PATH}:${HOME}/.dotnet:${HOME}/.dotnet/tools"
dotnet publish -c Release -o ./publish
echo "Publish hazır: ./publish  (FTP/IIS veya scp ile yükle)"
