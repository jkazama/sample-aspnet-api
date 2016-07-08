sample-aspnet-api
----

### はじめに

[ASP.NET Core](https://docs.asp.net/en/latest/) を元にしたアプリケーション開発サンプルです。

本サンプルは開発フレームワークというよりも、開発時の初期テンプレート的扱いで利用可能です。

本サンプルはAPI機能のみ有しています。
UI側の実装サンプルについては[sample-ui-vue](https://github.com/jkazama/sample-ui-vue) / [sample-ui-react](https://github.com/jkazama/sample-ui-react)を参照してください。

#### レイヤリングの考え方

オーソドックスな三層モデルですが、横断的な解釈としてインフラ層を考えています。

| レイヤ          | 特徴                                                        |
| -------------- | ----------------------------------------------------------- |
| UI             | ユースケース処理を公開(必要に応じてリモーティングや外部サイトを連携) |
| アプリケーション | ユースケース処理を集約(外部リソースアクセスも含む)                 |
| ドメイン        | 純粋なドメイン処理(外部リソースに依存しない)                      |
| インフラ        | DI コンテナや ORM 、各種ライブラリ、メッセージリソースの提供          |

UI 層の公開処理は通常 Razor を用いて行いますが、本サンプルでは異なる種類のクライアント利用を想定して RESTfulAPI での API 提供のみをおこないます。(利用クライアントは別途用意する必要があります)

#### ASP.NET Core の利用方針

ASP.NET Core は様々な利用方法が可能ですが、本サンプルでは以下のポリシーで利用します。

- 例外処理は基本上位委譲で終端 ( RestErrorFilter ) で捕捉定義
- ORM 実装として EntityFramework を利用
- 認証 / 認可は Identity を想定 (現在は未使用)

TBD

#### C#コーディング方針

TBD

#### パッケージ構成

パッケージ/リソース構成については以下を参照してください。

```
src
  Sample
    Context                         … インフラ層
    Controllers                     … UI層
    Models                          … ドメイン層
    Usecases                        … アプリケーション層
    Utils                           … 汎用ユーティリティ
    - config.json                   … プロジェクト設定情報
    - project.json                  … プロジェクト構成定義
    - DependencyInjection.cs        … DI定義
    - Program.cs                    … 実行可能な起動クラス
    - Startup.cs                    … 起動構成定義クラス
```

### サンプルユースケース

サンプルユースケースとしては以下のようなシンプルな流れを想定します。

- **口座残高100万円を持つ顧客**が出金依頼(発生 T, 受渡 T + 3)をする。
- **システム**が営業日を進める。
- **システム**が出金依頼を確定する。(確定させるまでは依頼取消行為を許容)
- **システム**が受渡日を迎えた入出金キャッシュフローを口座残高へ反映する。

### 環境構築手順

> サンプルなので、DB は SQLite を利用したファイルベースにしています。実際の開発用途では SQLServer などに変更してください。

#### Windows での環境構築手順

> 開発時は Visual Studio 2015 を入れてしまうのが簡単でオススメです。

https://www.microsoft.com/net/core#windows

環境構築後に Sample.sln をダブルクリックで Visual Studio 2015 からアプリケーションを実行できます。

#### Mac での環境構築手順

> 開発時は Visual Studio Code を入れてしまうのが良いですが、Mac 上での実行確認だけならコンソールベースで十分です。

https://www.microsoft.com/net/core#macos

動作確認手順は以下を参考にしてください。

- 環境構築後に src/Sample 直下へ移動する
- `dotnet restore` を実行してライブラリ構成をロード
- `export ASPNETCORE_ENVIRONMENT=Development` で起動変数を開発モードに
    - Startup.cs を見れば分かるようにファイル DB 構築と CORS 設定を有効にしています
- `dotnet run` を実行してアプリケーションを起動

※テスト確認したいなら test/Sample.Test 直下へ移動して、 `dotnet restore` `dotnet test` を実行してください。

### 補足解説（インフラ層）

#### 認証 / 認可

ASP.NET Core ベースの Identity 実装がまだ完全に消化できていないので一旦コメントアウト

#### 国際化

Resources 配下のクラスを利用。

> IStringLocalizer 経由が推奨？

### 残作業

- Identity を利用した認証/認可のサポート
- Repository絡みで起こるであろうリソースリークを想定してライフサイクルでの正確な挙動を調べる
- 非同期系の考慮 (async / await 適用すべき箇所をまだあまり調べられてない)
- キャッシングの追加
