export function convertirDuracionASegundos(duracionTexto) {
  if (!duracionTexto || duracionTexto.trim() === '') {
    return 0
  }

  const partes = duracionTexto.split(':')

  if (partes.length !== 2) {
    throw new Error('La duración debe tener el formato correcto.')
  }

  const minutos = Number(partes[0])
  const segundos = Number(partes[1])

  if (
    Number.isNaN(minutos) ||
    Number.isNaN(segundos) ||
    minutos < 0 ||
    segundos < 0 ||
    segundos >= 60
  ) {
    throw new Error("La duración ingresada no es válida.")
  }

  return minutos * 60 + segundos
}
