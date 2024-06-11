# Checkout Configuration
This action checks-out the repository, so the build workflow can access it.

Only a single commit is fetched by default, for the ref/SHA that is used in the configuration. Set `fetch-depth: 0` to fetch all history for all branches and tags. 

## Usage
```yaml
checkout:
    # Repository name with owner. For example 'Kalkwst/Didactic-octo-lamp'
    # Default: 'Kalkwst/Didactic-octo-lamp'
    repository: 'Kalkwst/Didactic-octo-lamp'

    # The branch, tag or SHA to checkout. This defaults to the default branch of your repository
    ref: ''

    # Relative path under the directory the build is initiated to place the repository
    path: ''

    # Whether to execute `git clean -ffdx && git reset --hard HEAD` before fetching
    # Default: false
    ignoreUntracked: ''

    # Number of commits to fetch. 0 indicates all history for all branches and tags
    # Default: 1
    fetchDepth: ''

    # Whether to fetch tags, even if fetch-depth > 0
    # Default: false
    fetchTags: ''

    # Partially clone against a given filter
    # Default: null
    filter: ''

    # The base URL of the Git Server instance that you are trying to clone from. Example URLs are https://github.com or
    # https://my-git-server.example.com
    gitServerUrl: ''

    # Whether to clear the $path and perform a clone of the repository
    # Default: false
    reclone: ''
```

## Scenarios
- [Fetch only the last commit of the repository](#fetch-only-the-last-commit-of-the-repository)
- [Fetch all history for all tags and branches]
### Fetch only the last commit of the repository

```json
"checkout": {
    "repository": "Kalkwst/Didactic-octo-lamp",
    "ref": "",
    "path": "",
    "ignoreUntracked": false,
    "fetchDepth": 1,
    "fetchTags": false,
    "filter": "",
    "gitServerUrl": "",
    "reclone": false
}
```

### Fetch all history for all tags and branches

```json
"checkout": {
    "repository": "Kalkwst/Didactic-octo-lamp",
    "ref": "",
    "path": "",
    "ignoreUntracked": false,
    "fetchDepth": 0,
    "fetchTags": true,
    "filter": "",
    "gitServerUrl": "",
    "reclone": false
}
```

### Avoid losing changes when checking out

```json
"checkout": {
    "repository": "Kalkwst/Didactic-octo-lamp",
    "ref": "",
    "path": "",
    "ignoreUntracked": true,
    "fetchDepth": 0,
    "fetchTags": true,
    "filter": "",
    "gitServerUrl": "",
    "reclone": false
}
```

### Checkout from private server
```json
"checkout": {
    "repository": "Kalkwst/Didactic-octo-lamp",
    "ref": "",
    "path": "",
    "ignoreUntracked": false,
    "fetchDepth": 0,
    "fetchTags": true,
    "filter": "",
    "gitServerUrl": "https://my-org.example.com",
    "reclone": false
}
```

### Get a fresh copy of the repository

```json
"checkout": {
    "repository": "Kalkwst/Didactic-octo-lamp",
    "ref": "",
    "path": "",
    "ignoreUntracked": false,
    "fetchDepth": 0,
    "fetchTags": true,
    "filter": "",
    "gitServerUrl": "",
    "reclone": true
}
```