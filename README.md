# ThirdPersonShooter

## Projeto de um sistema de armas em que as armas são facilmente modificavéis, pela utilização de scriptable objects, e as animações referentes a cada arma são controladas por meio de um rig builder.

Sobre:
=========================
O projeto foi realizado com base em um tutorial provido no YouTube pelo canal KiwiCoder, porem este foi modificado para fosse possivel a aplicação de uma variedade maior de mecânicas e melhor modularizado, de modo que partes do código possam ser reutilizadas em projetos futuros.

O projeto consta com cada arma sendo totalmente customizavel por meio de seu scriptable object, tanto como as animações que ela ira realizar quando equipada ou ao desequipa-la quanto o modo de disparo, podendo controlar a distancia que o tiro pode percorrer, o grau de queda e o fire rate.

Para a realização da animação de cada arma se utilizou de um rig builder, que anima diretamente o rig do personagem com posntos especificos da arma, como o grip, posição de arma desequipada, arma sem mirar e arma mirando. Com a troca de armas as animações que são tocadas são substituidas por um animation controller override, onde de inicio se tem as animações vazias, e assim que uma arma é equipada suas respectivas animações sobrescrevem as anteriores.
