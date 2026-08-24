#!/usr/bin/env bash
# Bu portföyü GitHub Repositories altına public repo olarak ekler.
# Kullanım:
#   export GH_TOKEN=ghp_xxx   # repo scope'lu classic PAT
#   bash scripts/publish-github.sh
set -euo pipefail

REPO_NAME="${REPO_NAME:-kisisel-portfolyo}"
OWNER="${OWNER:-HalilMertDeveli}"
VISIBILITY="${VISIBILITY:-public}"

if [[ -z "${GH_TOKEN:-}" ]]; then
  echo "GH_TOKEN yok. GitHub → Settings → Developer settings → Personal access tokens"
  echo "Classic token oluştur (scope: repo) ve export GH_TOKEN=... yap."
  exit 1
fi

export PATH="${PATH}:${HOME}/.dotnet:${HOME}/.dotnet/tools"
cd "$(dirname "$0")/.."

echo "$GH_TOKEN" | gh auth login --with-token
gh auth status

if gh repo view "$OWNER/$REPO_NAME" >/dev/null 2>&1; then
  echo "Repo zaten var: https://github.com/$OWNER/$REPO_NAME"
else
  gh repo create "$OWNER/$REPO_NAME" --"$VISIBILITY" --source=. --remote=github --description "Koyu temalı ASP.NET Core 8 kişisel portföy / CV sitesi"
fi

git remote remove github 2>/dev/null || true
git remote add github "https://x-access-token:${GH_TOKEN}@github.com/${OWNER}/${REPO_NAME}.git"
git push -u github HEAD:main

echo "Tamam → https://github.com/$OWNER/$REPO_NAME"
echo "Repositories: https://github.com/$OWNER?tab=repositories"
