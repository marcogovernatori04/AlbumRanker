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
        return 'bg-[#0B3326] text-yellow-300 font-bold'
    }

    if (puntuacion >= 9) {
        return 'bg-[#166437] text-white border-[#000000]'
    }

    if (puntuacion >= 8) {
        return 'bg-[#32C36A] text-neutral-950'
    }

    if (puntuacion >= 7) {
        return 'bg-[#F2CD2C] text-neutral-950'
    }

    if (puntuacion >= 6) {
        return 'bg-[#EB950A] text-neutral-950'
    }

    if (puntuacion >= 5) {
        return 'bg-[#E33926] text-white border-[#000000]'
    }

    return 'bg-[#6C180E] text-white border-[#000000]'
}
