```xaml
 <DrawingBrush x:Key="MyGridBrushResource" 
                      Viewport="0,0,40,40" 
                      ViewportUnits="Absolute"
                      TileMode="Tile">
            <DrawingBrush.Drawing>
                <DrawingGroup>
                    <DrawingGroup.Children>
                        <GeometryDrawing Geometry="M0,0 L10,0 10,0.1, 0,0.1Z" Brush="Gray" />
                        <GeometryDrawing Geometry="M0,0 L0,10 0.1,10, 0.1,0Z" Brush="Gray" />
                    </DrawingGroup.Children>
                </DrawingGroup>
            </DrawingBrush.Drawing>
        </DrawingBrush>
```

Viewport = "0,0,width,height"

```xaml
You can also use DrawingBrush to draw the grid lines in the xaml like this:


Copy
 <Window.Resources>  
        <DrawingBrush x:Key="MyGridBrushResource" Viewport="0,0,10,10" ViewportUnits="Absolute" TileMode="Tile">  
            <DrawingBrush.Drawing>  
                <DrawingGroup>  
                    <DrawingGroup.Children>  
                        <GeometryDrawing Brush="#99FFFFFF">  
                            <GeometryDrawing.Geometry>  
                                <RectangleGeometry Rect="0,0,1,1" />  
                            </GeometryDrawing.Geometry>  
                        </GeometryDrawing>  
                        <GeometryDrawing Geometry="M0,0 L1,0 1,0.1, 0,0.1Z" Brush="#66CCCCFF" />  
                        <GeometryDrawing Geometry="M0,0 L0,1 0.1,1, 0.1,0Z" Brush="#66CCCCFF" />  
                    </DrawingGroup.Children>  
                </DrawingGroup>  
            </DrawingBrush.Drawing>  
        </DrawingBrush>  
    </Window.Resources>  
    <Grid>  
        <Image Width="200" Height="200">  
            <Image.Source>  
                <DrawingImage>  
                    <DrawingImage.Drawing>  
                        <DrawingGroup>  
                            <DrawingGroup.Children>  
                                <GeometryDrawing Brush="{StaticResource MyGridBrushResource}">  
                                    <GeometryDrawing.Geometry>  
                                        <RectangleGeometry Rect="0,0,101,101" />  
                                    </GeometryDrawing.Geometry>  
                                </GeometryDrawing>  
                            </DrawingGroup.Children>  
                        </DrawingGroup>  
                    </DrawingImage.Drawing>  
                </DrawingImage>  
            </Image.Source>  
        </Image>  
    </Grid>  
</Window>  
```