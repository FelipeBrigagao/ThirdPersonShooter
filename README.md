# ThirdPersonShooter

## Projeto de um sistema de armas em que as armas são facilmente modificáveis, pela utilização de scriptable objects, e as animações referentes a cada arma são controladas por meio de um rig builder.

Sobre:
=========================
-Geral:

O projeto foi realizado com base em um tutorial provido no YouTube pelo canal KiwiCoder, porém este foi modificado para fosse possível a aplicação de uma variedade maior de mecânicas e melhor modularizado, de modo que partes do código possam ser reutilizadas em projetos futuros.

O projeto consta com cada arma sendo totalmente customizavel por meio de seu scriptable object, tanto como as animações que ela irá realizar quando equipada ou ao desequipa-la quanto o modo de disparo, podendo controlar a distância que o tiro pode percorrer, o grau de queda e o fire rate.

Para a realização da animação de cada arma se utilizou de um rig builder, que anima diretamente o rig do personagem com pontos específicos da arma, como o grip, posição de arma desequipada, arma sem mirar e arma mirando. Com a troca de armas as animações que são tocadas são substituídas por um animation controller override, onde de início se tem as animações vazias, e assim que uma arma é equipada suas respectivas animações sobrescrevem as anteriores.

-Assets:

Os assets utilizados para esse projetos foram baixados da Unity Asset Store, esses são:
  
  - POLYGON Starter Pack
  - Unity Particle Pack 5.x
