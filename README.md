# 20140822ex2
## はじめに
このマクロは、画面に表示された記録ボタンを1回押すごとに、  
一台のKinectとArduinoから、Kinectのサンプリングレート(約30Hz)で  
以下のデータを100こずつ取得します。  

* Color画像
* Depth画像(640x480)
* SkeletonStreamで取得できる各関節の座標をDepth座標系に変換した値
* Color画像座標系からDepth座標系の変換表
* Arduinoに接続された6このセンサの値（CenterOfGravityが必要です）

## 環境
以下の環境で書きました。

* OS: Windows7 / 32bit
* Visual Studio 2010

## 使用方法
以下のファイルを実行してください。
20140822ex2 > a20140511 > bin > Release > a20140511.exe  
  
Arduinoが接続されていないとプログラムを実行できません。

## 編集する際の注意
VisualStudioでCenterOfGravity（DLLファイル）を参照追加してください。
