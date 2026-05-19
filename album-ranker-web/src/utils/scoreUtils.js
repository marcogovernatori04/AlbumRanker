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
        return 'bg-[#0E6EAA] text-white'
    }

    if (puntuacion >= 9) {
        return 'bg-[#166437] text-white'
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
        return 'bg-[#E33926] text-white'
    }

    return 'bg-[#8B0000] text-white'
}
