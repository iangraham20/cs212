   M S B A M L   `   `   `  ����   Fractal     SquareNamespace.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     � Title$  Fractal Project��$	��543��$	��863�� � WindowStyle$ ThreeDBorderWindow=�.��� ���	#FFF4FFD8.+��� -   $��canvas��$	��480��$
��Left=�$��193,12,12,0q�$	��Top=�$	��640��
�������}�. �� -   $��
sizeSlider��$��24��$
��Left=�$��	12,85,0,0q�$	��Top=�$	��175�� � Value$	 150Q� � Maximum$	 240Q� � Minimum$ 10Q��� -   $��reduxSlider��$��23��$
��Left=�$��
12,151,0,0q�$	��Top=�$	��175��$ 1.0325Q�$
 1.15Q�$ 1.0325Q��� -   $��depthSlider��$��23��$
��Left=�$��
10,309,0,0q�$	��Top=�$	��175��$ 10Q�$	 100Q�$ 0Q��� -   $��	redSlider��$��23��$
��Left=�$��
12,241,0,0q�$	��Top=�$��53��$ 0Q�$	 255Q�$ 0Q��� -   $��greenSlider��$��23��$
��Left=�$��
76,241,0,0q�$	��Top=�$��53��$ 0Q�$	 255Q�$ 0Q��� -   $��
blueSlider��$��23��$
��Left=�$��134,241,0,0q�$	��Top=�$��53��$	 255Q�$	 255Q�$ 0Q��� -	   $��Button1��$
��Draw��$��37��$
��Left=�$��
41,361,0,0q�$	��Top=�$	��101���� -
   $��label1��$
��Size��$��28��$��12,57,668,0q�$	��Top=�$��Center=�$	��175���� -   $��label2��$��Redux��$��28��$
��Left=�$��
12,123,0,0q�$	��Top=�$	��175���� -   $��label3��$��Depth��$��28��$
��Left=�$��
10,281,0,0q�$	��Top=�$	��175���� -   $��label4��$[��UDepth Value: {Binding ElementName=DepthSlider, Path=Value, Converter=stringconverter}��$��28��$
��Left=�$��
10,281,0,0q�$	��Top=�$	��175���� -   $��label5��$��Color��$��28��$
��Left=�$��
12,188,0,0q�$	��Top=�$	��175���� -   $��label6��$��R��$��28��$
��Left=�$��
12,213,0,0q�$	��Top=�$��53���� -   $��label7��$��G��$��28��$
��Left=�$��
76,213,0,0q�$	��Top=�$��53���� -   $��label8��$��B��$��28��$
��Left=�$��134,213,0,0q�$	��Top=�$��53��