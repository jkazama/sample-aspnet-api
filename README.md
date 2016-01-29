sample-aspnet-core
----

### はじめに

[ASP.NET Core](http://docs.asp.net/en/latest/conceptual-overview/dotnetcore.html) を元にしたアプリケーション開発サンプルです。

本サンプルは開発フレームワークというよりも、開発時の初期テンプレート的扱いで利用可能です。
依存ライブラリも純粋なOSSで閉じているため、学習用ととしても手軽に利用できます。

本サンプルはAPI機能のみ有しています。
UI側の実装サンプルについては[sample-ui-vue](https://github.com/jkazama/sample-ui-vue) / [sample-ui-react](https://github.com/jkazama/sample-ui-react)を参照してください。

---

ASP.NET Core はまだRC版という事もあり、API等の仕様は完全ではありません。  
本来あるべき実装(認証/国際化/監査 等)も入れられていないので、簡単な動作確認用途で利用してください。

> 不足機能は徐々に追加していく予定  
> 現時点で一般的な ASP.NET 4.5 版は別レポジトリで準備する予定  


#### レイヤリングの考え方

オーソドックスな三層モデルですが、横断的な解釈としてインフラ層を考えています。

| レイヤ          | 特徴                                                        |
| -------------- | ----------------------------------------------------------- |
| UI             | ユースケース処理を公開(必要に応じてリモーティングや外部サイトを連携) |
| アプリケーション | ユースケース処理を集約(外部リソースアクセスも含む)                 |
| ドメイン        | 純粋なドメイン処理(外部リソースに依存しない)                      |
| インフラ        | DIコンテナやORM、各種ライブラリ、メッセージリソースの提供          |

UI層の公開処理は通常Razorを用いて行いますが、本サンプルでは異なる種類のクライアント利用を想定してRESTfulAPIでのAPI提供のみをおこないます。(利用クライアントは別途用意する必要があります)

#### ASP.NET Core の利用方針

ASP.NET Core は様々な利用方法が可能ですが、本サンプルでは以下のポリシーで利用します。

- 例外処理は基本上位委譲で終端(RestErrorFilter)で捕捉定義
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
    - project.json                  … プロジェクト構成定義
    - DependencyInjection           … DI定義
    - Startup.cs                    … 実行可能な起動クラス
```

### サンプルユースケース

サンプルユースケースとしては以下のようなシンプルな流れを想定します。

- **口座残高100万円を持つ顧客**が出金依頼(発生 T, 受渡 T + 3)をする。
- **システム**が営業日を進める。
- **システム**が出金依頼を確定する。(確定させるまでは依頼取消行為を許容)
- **システム**が受渡日を迎えた入出金キャッシュフローを口座残高へ反映する。

### 環境構築手順

#### Windows での環境構築手順

> 開発時は Visual Studio 2015 を入れてしまう手順 ( Install ASP.NET 5 with Visual Studio ) が簡単でオススメです。

https://docs.asp.net/en/latest/getting-started/installing-on-windows.html

環境構築後に Sample.sln をダブルクリックで Visual Studio 2015 からアプリケーションを実行できます。

#### Mac での環境構築手順

> 開発時は Visual Studio Code を入れてしまう手順 ( Install ASP.NET 5 with Visual Studio Code ) が良いですが、Mac 上での実行確認だけならコンソールベース ( Install ASP.NET 5 from the command-line ) でも十分です。

https://docs.asp.net/en/latest/getting-started/installing-on-mac.html

環境構築後に src/Sample 直下へ移動して、 `dnx web` を実行すればアプリケーションが起動します。  
※テスト確認したいなら test/Sample.Test 直下へ移動して、 `dnx test` を実行してください。

### 補足解説（インフラ層）

#### 認証 / 認可

ASP.NET Core ベースの Identity 実装がまだ完全に消化できていないので一旦コメントアウト

#### 国際化

現状試したけど resx がうまく読み込めなくて挫折。以下議論のオチが反映されてから再度挑戦。
https://github.com/aspnet/Home/issues/1142

### 残作業

- Identity を利用した認証/認可のサポート
- Repository絡みで起こるであろうリソースリークを想定してライフサイクルでの正確な挙動を調べる
- 非同期系の考慮 (async / await 適用すべき箇所をまだあまり調べられてない)
- キャッシングの追加
