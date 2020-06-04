#### mathematics

##### quaternion
+ 构造
   + quaternion.identity
   + quaternion(f3x3 mat/f4 四元数/f4x4 orthonormalMat)
+ Equals(obj装箱/quaternion)，GetHashCode
+ 旋转
   + quaternion.AxisAngles(up,angle) 喜大普奔！
   + Euler各种重载
   + LookRotation(Safe-unitLength)(f3 forward, f3 up)
   + RotateX/Y/Z(f1)

##### noise
文档Description全空，是时候展示联想艺术了
+ cellular
+ cnoise 中心分布(uniform)
+ pnoise(pos,represent平均值) 泊松分布
+ sr(d)noise(f2 pos, f1 rot)
+ psr(d)noise(pos,period,rot) 
+ snoise(vector,out gradient)