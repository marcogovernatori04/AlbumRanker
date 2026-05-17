export function formatearPuntuacion(puntuacion) {
    if (puntuacion === null || puntuacion === undefined) return '-'

    return Number(puntuacion).toLocaleString('es-AR', {
        minimumFractionDigits: puntuacion % 1 === 0 ? 0 : 1,
        maximumFractionDigits: 1
    })
}

export function clasePuntuacion(puntuacion) {
    if (puntuacion === null || puntuacion === undefined) {
        return 'bg-neutral-800 text-white'
    }

    if (puntuacion >= 9.75) {
        return 'bg-[#20a0ed] text-white'
    }

    if (puntuacion >= 9) {
        return 'bg-[#176a3a] text-white'
    }

    if (puntuacion >= 8) {
        return 'bg-[#2eb161] text-white'
    }

    if (puntuacion >= 7) {
        return 'bg-[#f3d03c] text-neutral-950'
    }

    if (puntuacion >= 6) {
        return 'bg-[#f59b0f] text-neutral-950'
    }

    if (puntuacion >= 5) {
        return 'bg-[#e64d3b] text-white'
    }

    return 'bg-[#613976] text-white'
}