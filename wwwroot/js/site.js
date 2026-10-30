// Halil Mert Develi — portfolio interactions.
// Small, dependency-free, and every effect degrades to a static page.
(() => {
  "use strict";

  const root = document.documentElement;
  const reduceMotion = window.matchMedia("(prefers-reduced-motion: reduce)");
  const finePointer = window.matchMedia("(hover: hover) and (pointer: fine)");

  /* ---------- Navigation ---------- */
  const nav = document.querySelector("[data-nav]");
  const toggle = document.querySelector("[data-nav-toggle]");
  const menu = document.querySelector("[data-nav-menu]");

  if (nav) {
    const onScroll = () => nav.classList.toggle("is-scrolled", window.scrollY > 12);
    onScroll();
    window.addEventListener("scroll", onScroll, { passive: true });
  }

  if (nav && toggle && menu) {
    const label = toggle.querySelector(".sr-only");
    const setOpen = (open) => {
      nav.classList.toggle("is-open", open);
      toggle.setAttribute("aria-expanded", String(open));
      document.body.style.overflow = open ? "hidden" : "";
      if (label) label.textContent = open ? label.dataset.labelClose : label.dataset.labelOpen;
    };

    toggle.addEventListener("click", () => setOpen(!nav.classList.contains("is-open")));
    menu.querySelectorAll("a").forEach((link) => link.addEventListener("click", () => setOpen(false)));
    document.addEventListener("keydown", (e) => {
      if (e.key === "Escape" && nav.classList.contains("is-open")) {
        setOpen(false);
        toggle.focus();
      }
    });
    window.matchMedia("(min-width: 961px)").addEventListener("change", (e) => {
      if (e.matches) setOpen(false);
    });
  }

  /* ---------- Current section in nav ---------- */
  const navLinks = [...document.querySelectorAll("[data-nav-link]")];
  const sections = navLinks
    .map((link) => document.querySelector(link.getAttribute("href")))
    .filter(Boolean);

  if ("IntersectionObserver" in window && sections.length) {
    const spy = new IntersectionObserver(
      (entries) => {
        entries.forEach((entry) => {
          if (!entry.isIntersecting) return;
          navLinks.forEach((link) => {
            const active = link.getAttribute("href") === `#${entry.target.id}`;
            link.classList.toggle("is-current", active);
            if (active) link.setAttribute("aria-current", "location");
            else link.removeAttribute("aria-current");
          });
        });
      },
      { rootMargin: "-45% 0px -50% 0px" }
    );
    sections.forEach((s) => spy.observe(s));
  }

  /* ---------- Reveal on scroll ---------- */
  const revealables = document.querySelectorAll("main [data-reveal]:not(.hero [data-reveal])");
  if ("IntersectionObserver" in window && !reduceMotion.matches) {
    const io = new IntersectionObserver(
      (entries) => {
        entries.forEach((entry) => {
          if (!entry.isIntersecting) return;
          entry.target.classList.add("is-visible");
          io.unobserve(entry.target);
        });
      },
      { rootMargin: "0px 0px -8% 0px", threshold: 0.08 }
    );

    // Stagger siblings in grids so cards arrive in sequence.
    revealables.forEach((el) => {
      const siblings = el.parentElement ? [...el.parentElement.children].filter((c) => c.hasAttribute("data-reveal")) : [];
      const index = siblings.indexOf(el);
      if (index > 0) el.style.setProperty("--delay", `${Math.min(index, 5) * 70}ms`);
      io.observe(el);
    });
  } else {
    revealables.forEach((el) => el.classList.add("is-visible"));
      }

  /* ---------- Subtle depth on cards (fine pointers only) ---------- */
  if (finePointer.matches && !reduceMotion.matches) {
    document.querySelectorAll("[data-tilt]").forEach((el) => {
      const max = el.classList.contains("project") ? 2.5 : 3.5;
      let frame = 0;
      el.addEventListener("pointermove", (e) => {
        cancelAnimationFrame(frame);
        frame = requestAnimationFrame(() => {
          const r = el.getBoundingClientRect();
          const x = (e.clientX - r.left) / r.width - 0.5;
          const y = (e.clientY - r.top) / r.height - 0.5;
          el.style.setProperty("--ry", `${(x * max).toFixed(2)}deg`);
          el.style.setProperty("--rx", `${(-y * max).toFixed(2)}deg`);
        });
      });
      el.addEventListener("pointerleave", () => {
        cancelAnimationFrame(frame);
        el.style.setProperty("--ry", "0deg");
        el.style.setProperty("--rx", "0deg");
      });
    });
  }

  const animate = !reduceMotion.matches;
  const onceVisible = (el, fn, options = { threshold: 0.3 }) => {
    if (!("IntersectionObserver" in window)) return fn(el);
    const io = new IntersectionObserver(([entry]) => {
      if (!entry.isIntersecting) return;
      io.disconnect();
      fn(el);
    }, options);
    io.observe(el);
  };

  /* ---------- Scroll progress ---------- */
  const progress = document.querySelector("[data-progress]");
  if (progress) {
    let queued = false;
    const paint = () => {
      queued = false;
      const max = root.scrollHeight - window.innerHeight;
      progress.style.transform = `scaleX(${max > 0 ? Math.min(window.scrollY / max, 1).toFixed(4) : 0})`;
    };
    const queue = () => {
      if (queued) return;
      queued = true;
      requestAnimationFrame(paint);
    };
    paint();
    window.addEventListener("scroll", queue, { passive: true });
    window.addEventListener("resize", queue, { passive: true });
  }

  /* ---------- Cursor light: lights up the LED dots under the pointer ---------- */
  if (finePointer.matches && animate) {
    document.querySelectorAll("[data-spot]").forEach((el) => {
      el.addEventListener("pointermove", (e) => {
        const r = el.getBoundingClientRect();
        el.style.setProperty("--mx", `${(e.clientX - r.left).toFixed(0)}px`);
        el.style.setProperty("--my", `${(e.clientY - r.top).toFixed(0)}px`);
      }, { passive: true });
    });
  }

  /* ---------- Counters ---------- */
  // The final number is already in the markup, so no-JS and reduced motion show the real value.
  document.querySelectorAll("[data-count]").forEach((el) => {
    if (!animate) return;
    const target = Number(el.dataset.count);
    const format = new Intl.NumberFormat(root.lang || "tr");
    onceVisible(el, () => {
      const started = performance.now();
      const duration = 1400;
      const tick = (now) => {
        const p = Math.min((now - started) / duration, 1);
        el.textContent = format.format(Math.round(target * (1 - Math.pow(1 - p, 4))));
        if (p < 1) requestAnimationFrame(tick);
      };
      requestAnimationFrame(tick);
    });
  });

  /* ---------- Eyebrows decode like a controller boot line ---------- */
  if (animate) {
    const glyphs = "01#/<>_+*";
    document.querySelectorAll(".eyebrow").forEach((el) => {
      const node = [...el.childNodes].reverse().find((n) => n.nodeType === Node.TEXT_NODE && n.textContent.trim());
      if (!node) return;
      const text = node.textContent;
      onceVisible(el, () => {
        const started = performance.now();
        const tick = (now) => {
          const shown = Math.floor((now - started) / 28);
          if (shown >= text.length) {
            node.textContent = text;
            return;
          }
          node.textContent = text.slice(0, shown) + [...text.slice(shown)].map((c) => (c === " " ? " " : glyphs[Math.floor(Math.random() * glyphs.length)])).join("");
          requestAnimationFrame(tick);
        };
        requestAnimationFrame(tick);
      }, { threshold: 0.6 });
    });
  }

  /* ---------- Project filter ---------- */
  const filterBar = document.querySelector("[data-filters]");
  if (filterBar) {
    const buttons = [...filterBar.querySelectorAll("[data-filter]")];
    const items = [...document.querySelectorAll("[data-tags]")];
    const count = document.querySelector("[data-filter-count]");
    const apply = (filter) => {
      let visible = 0;
      items.forEach((item) => {
        const show = filter === "all" || item.dataset.tags.split(" ").includes(filter);
        item.hidden = !show;
        if (show) visible += 1;
        // Reveal immediately: a card that was filtered out never crossed the observer.
        item.classList.add("is-visible");
      });
      buttons.forEach((b) => b.setAttribute("aria-pressed", String(b.dataset.filter === filter)));
      if (count) count.textContent = String(visible);
    };
    buttons.forEach((button) => {
      button.addEventListener("click", () => {
        const run = () => apply(button.dataset.filter);
        if (animate && !document.hidden && document.startViewTransition) {
          // A skipped transition rejects these promises; the filter has still been applied.
          const transition = document.startViewTransition(run);
          transition.ready.catch(() => {});
          transition.finished.catch(() => {});
        } else {
          run();
        }
      });
    });
  }

  /* ---------- LED ticker ---------- */
  // Rasterises the text once, then scrolls it across a dot grid like a real LED sign.
  document.querySelectorAll("[data-ticker]").forEach((ticker) => {
    const ctx = ticker.getContext("2d", { alpha: true });
    const rows = 15;
    const palette = [[243, 181, 98], [185, 163, 255], [141, 182, 255], [95, 211, 160]];
    let lit = new Uint8Array(0), textCols = 1, gap = 5, cols = 0, w = 0, dpr = 1;
    let running = false, visible = true, raf = 0, offset = 0, last = 0;

    const rasterise = () => {
      const source = document.createElement("canvas");
      const sctx = source.getContext("2d", { willReadFrequently: true });
      const font = `700 ${rows - 2}px ${getComputedStyle(root).getPropertyValue("--font-sans")}`;
      sctx.font = font;
      const text = ticker.dataset.ticker;
      textCols = Math.ceil(sctx.measureText(text).width) + 40;
      source.width = textCols;
      source.height = rows;
      sctx.font = font;
      sctx.textBaseline = "middle";
      sctx.fillStyle = "#fff";
      sctx.fillText(text, 0, rows / 2 + 1);
      const data = sctx.getImageData(0, 0, textCols, rows).data;
      lit = new Uint8Array(textCols * rows);
      for (let i = 0; i < lit.length; i++) lit[i] = data[i * 4 + 3] > 96 ? 1 : 0;
    };

    const resize = () => {
      gap = window.innerWidth < 640 ? 4 : 5;
      dpr = Math.min(window.devicePixelRatio || 1, 2);
      w = ticker.getBoundingClientRect().width;
      const h = rows * gap;
      ticker.style.height = `${h}px`;
      ticker.width = Math.round(w * dpr);
      ticker.height = Math.round(h * dpr);
      ctx.setTransform(dpr, 0, 0, dpr, 0, 0);
      cols = Math.ceil(w / gap) + 1;
      draw();
    };

    const draw = () => {
      ctx.clearRect(0, 0, w, rows * gap);
      const shift = Math.floor(offset);
      const size = gap - 1.5;
      const span = Math.max(cols, 1);
      for (let x = 0; x < cols; x++) {
        const col = (x + shift) % textCols;
        const band = ((col / textCols) * palette.length * 3) % palette.length;
        const [r, g, b] = palette[Math.floor(band)];
        const edge = Math.min(x, span - x) / 14;
        const fade = Math.min(edge, 1);
        for (let y = 0; y < rows; y++) {
          const on = lit[y * textCols + col];
          ctx.fillStyle = on ? `rgba(${r},${g},${b},${(0.95 * fade).toFixed(3)})` : "rgba(255,255,255,0.045)";
          ctx.fillRect(x * gap, y * gap, size, size);
        }
      }
    };

    const loop = (now) => {
      // A frame's timestamp can precede the performance.now() taken in play(), so clamp at zero.
      offset = (offset + Math.max(0, Math.min(now - last, 50)) * 0.022) % textCols;
      last = now;
      draw();
      raf = requestAnimationFrame(loop);
    };

    const play = () => {
      if (running || reduceMotion.matches || !visible || document.hidden) return;
      running = true;
      last = performance.now();
      raf = requestAnimationFrame(loop);
    };

    const pause = () => {
      running = false;
      cancelAnimationFrame(raf);
    };

    const boot = () => {
      rasterise();
      resize();
      play();
    };

    // The webfont changes the letter shapes, so rasterise after it has loaded.
    (document.fonts && document.fonts.ready ? document.fonts.ready : Promise.resolve()).then(boot);
    window.addEventListener("resize", () => {
      clearTimeout(resize.t);
      resize.t = setTimeout(resize, 120);
    }, { passive: true });
    if ("IntersectionObserver" in window) {
      new IntersectionObserver(([entry]) => {
        visible = entry.isIntersecting;
        visible ? play() : pause();
      }).observe(ticker);
    }
    document.addEventListener("visibilitychange", () => (document.hidden ? pause() : play()));
  });

  /* ---------- Copy email ---------- */
  const status = document.querySelector("[data-copy-status]");
  document.querySelectorAll("[data-copy]").forEach((btn) => {
    btn.addEventListener("click", async () => {
      const value = btn.getAttribute("data-copy");
      const label = btn.querySelector("[data-copy-label]");
      const original = label ? label.textContent : "";
      try {
        await navigator.clipboard.writeText(value);
        if (label) label.textContent = btn.dataset.copiedLabel;
        if (status) status.textContent = btn.dataset.copiedLabel;
        setTimeout(() => {
          if (label) label.textContent = original;
        }, 2000);
      } catch {
        window.location.href = `mailto:${value}`;
      }
    });
  });

  /* ---------- Dialogs: recruiter brief and engineering sheets ---------- */
  document.querySelectorAll("[data-open]").forEach((button) => {
    button.addEventListener("click", () => {
      const dialog = document.getElementById(button.dataset.open);
      if (dialog && dialog.showModal) dialog.showModal();
    });
  });
  document.querySelectorAll("dialog.sheet").forEach((dialog) => {
    dialog.querySelectorAll("[data-close]").forEach((b) => b.addEventListener("click", () => dialog.close()));
    // A click on the backdrop lands on the dialog element itself.
    dialog.addEventListener("click", (e) => {
      if (e.target === dialog) dialog.close();
    });
  });
  document.querySelectorAll("[data-print]").forEach((b) => b.addEventListener("click", () => window.print()));

  /* ---------- Stacked product panels ---------- */
  const panels = [...document.querySelectorAll("[data-product]")];
  if (panels.length) {
    // A panel taller than the screen sticks by its bottom edge, so all of it can be read before the next one covers it.
    const measure = () => {
      panels.forEach((panel) => {
        panel.style.setProperty("--stick", `${Math.min(0, window.innerHeight - panel.offsetHeight)}px`);
      });
    };
    measure();
    window.addEventListener("resize", measure, { passive: true });
    if ("ResizeObserver" in window) {
      const ro = new ResizeObserver(measure);
      panels.forEach((panel) => ro.observe(panel));
    }

    if (animate) {
      let queued = false;
      const paint = () => {
        queued = false;
        panels.forEach((panel, i) => {
          const next = panels[i + 1];
          const covered = next ? 1 - next.getBoundingClientRect().top / window.innerHeight : 0;
          const cover = Math.max(0, Math.min(1, covered));
          panel.style.setProperty("--cover", cover.toFixed(3));
          const top = panel.getBoundingClientRect().top / window.innerHeight;
          panel.style.setProperty("--drift", `${(top * 12).toFixed(2)}vw`);
        });
      };
      const queue = () => {
        if (queued) return;
        queued = true;
        requestAnimationFrame(paint);
      };
      paint();
      window.addEventListener("scroll", queue, { passive: true });
    }
  }

  /* ---------- Demo: Mevora music match ---------- */
  document.querySelectorAll("[data-demo-match]").forEach((demo) => {
    const sliders = [...demo.querySelectorAll("input[type=range]")];
    const ring = demo.querySelector(".match__ring");
    const score = demo.querySelector("[data-match-score]");
    const why = demo.querySelector("[data-match-why]");
    const update = () => {
      const total = sliders.reduce((sum, s) => sum + (Number(s.dataset.weight) * Number(s.value)) / 100, 0);
      const best = sliders.reduce((a, b) => (Number(b.value) * Number(b.dataset.weight) > Number(a.value) * Number(a.dataset.weight) ? b : a));
      ring.style.setProperty("--score", total.toFixed(1));
      score.textContent = String(Math.round(total));
      why.textContent = `${demo.dataset.bestLabel} ${best.dataset.name}`;
      sliders.forEach((s) => s.style.setProperty("--fill", `${s.value}%`));
    };
    sliders.forEach((s) => s.addEventListener("input", update));
    update();
  });

  /* ---------- Demo: LED wall configurator ---------- */
  document.querySelectorAll("[data-demo-wall]").forEach((demo) => {
    const widthInput = demo.querySelector("[data-wall-width]");
    const heightInput = demo.querySelector("[data-wall-height]");
    const canvas = demo.querySelector("[data-wall-preview]");
    const ctx = canvas.getContext("2d");
    const buttons = [...demo.querySelectorAll("[data-pitch]")];
    const set = (key, value) => (demo.querySelector(`[data-wall-${key}]`).textContent = value);
    const locale = root.lang || "tr";
    let pitch = 2.5, running = false, visible = false, raf = 0;

    const compute = () => {
      const w = Number(widthInput.value);
      const h = Number(heightInput.value);
      const px = Math.round((w * 1000) / pitch);
      const py = Math.round((h * 1000) / pitch);
      set("w", w.toLocaleString(locale));
      set("h", h.toLocaleString(locale));
      set("res", `${px.toLocaleString(locale)} × ${py.toLocaleString(locale)}`);
      set("px", `${((px * py) / 1e6).toLocaleString(locale, { maximumFractionDigits: 1 })} M`);
      set("cab", String(Math.ceil(w / 0.5) * Math.ceil(h / 0.5)));
      widthInput.style.setProperty("--fill", `${((w - 1) / 9) * 100}%`);
      heightInput.style.setProperty("--fill", `${((h - 1) / 4) * 100}%`);
    };

    // The dots are not to scale; a coarser pitch simply reads as bigger dots, as it does in a real wall.
    const draw = (now) => {
      const rect = canvas.getBoundingClientRect();
      if (!rect.width) return;
      const dpr = Math.min(window.devicePixelRatio || 1, 2);
      if (canvas.width !== Math.round(rect.width * dpr)) {
        canvas.width = Math.round(rect.width * dpr);
        canvas.height = Math.round(rect.height * dpr);
      }
      ctx.setTransform(dpr, 0, 0, dpr, 0, 0);
      ctx.clearRect(0, 0, rect.width, rect.height);
      const w = Number(widthInput.value);
      const h = Number(heightInput.value);
      const scale = Math.min((rect.width - 16) / 10, (rect.height - 16) / 5);
      const wallW = w * scale;
      const wallH = h * scale;
      const x0 = (rect.width - wallW) / 2;
      const y0 = (rect.height - wallH) / 2;
      const gap = 2 + pitch * 1.1;
      const t = now / 1000;
      for (let x = x0; x < x0 + wallW; x += gap) {
        for (let y = y0; y < y0 + wallH; y += gap) {
          const wave = Math.sin((x - x0) * 0.022 + t * 1.3) + Math.cos((y - y0) * 0.03 - t * 0.9);
          const hue = 34 + wave * 26;
          const light = 42 + wave * 15;
          ctx.fillStyle = `hsl(${hue.toFixed(0)} 92% ${light.toFixed(0)}%)`;
          ctx.fillRect(x, y, gap - 1, gap - 1);
        }
      }
      // Cabinet seams, every half metre.
      ctx.strokeStyle = "rgba(0,0,0,0.55)";
      ctx.lineWidth = 1;
      ctx.beginPath();
      for (let i = 0.5; i < w; i += 0.5) {
        ctx.moveTo(x0 + i * scale, y0);
        ctx.lineTo(x0 + i * scale, y0 + wallH);
      }
      for (let i = 0.5; i < h; i += 0.5) {
        ctx.moveTo(x0, y0 + i * scale);
        ctx.lineTo(x0 + wallW, y0 + i * scale);
      }
      ctx.stroke();
    };

    const loop = (now) => {
      draw(now);
      raf = requestAnimationFrame(loop);
    };
    const play = () => {
      if (running || !visible || !animate || document.hidden) return;
      running = true;
      raf = requestAnimationFrame(loop);
    };
    const pause = () => {
      running = false;
      cancelAnimationFrame(raf);
    };

    const refresh = () => {
      compute();
      draw(performance.now());
    };
    widthInput.addEventListener("input", refresh);
    heightInput.addEventListener("input", refresh);
    buttons.forEach((button) => {
      button.addEventListener("click", () => {
        pitch = Number(button.dataset.pitch);
        buttons.forEach((b) => b.setAttribute("aria-pressed", String(b === button)));
        refresh();
      });
    });
    refresh();
    window.addEventListener("resize", () => draw(performance.now()), { passive: true });
    if ("IntersectionObserver" in window) {
      new IntersectionObserver(([entry]) => {
        visible = entry.isIntersecting;
        if (visible) {
          draw(performance.now());
          play();
        } else {
          pause();
        }
      }).observe(canvas);
    }
    document.addEventListener("visibilitychange", () => (document.hidden ? pause() : play()));
  });

  /* ---------- Demo: Motor fitment search ---------- */
  document.querySelectorAll("[data-demo-motor]").forEach((demo) => {
    const brand = demo.querySelector("[data-fit-brand]");
    const model = demo.querySelector("[data-fit-model]");
    const year = demo.querySelector("[data-fit-year]");
    const count = demo.querySelector("[data-fit-count]");
    const parts = [...demo.querySelectorAll("[data-models]")];
    const options = [...model.options];

    const filter = () => {
      let shown = 0;
      parts.forEach((part) => {
        const fits = part.dataset.models.split(" ").includes(model.value) && Number(year.value) >= Number(part.dataset.from);
        part.hidden = !fits;
        if (fits) shown += 1;
      });
      count.textContent = String(shown);
    };
    const syncModels = () => {
      options.forEach((o) => {
        const match = o.dataset.brand === brand.value;
        o.hidden = !match;
        o.disabled = !match;
      });
      const first = options.find((o) => o.dataset.brand === brand.value);
      if (model.selectedOptions[0] && model.selectedOptions[0].dataset.brand !== brand.value) model.value = first.value;
      filter();
    };
    brand.addEventListener("change", syncModels);
    model.addEventListener("change", filter);
    year.addEventListener("change", filter);
    syncModels();
  });

  /* ---------- Demo: bot conversation ---------- */
  // Without script or with reduced motion the whole conversation is simply shown.
  document.querySelectorAll("[data-demo-bot]").forEach((demo) => {
    const chat = demo.querySelector("[data-bot-chat]");
    const lines = [...chat.children];
    const slots = [...demo.querySelectorAll("[data-slot-key]")];
    const replay = demo.querySelector("[data-bot-replay]");
    let timer = 0, index = 0;

    const fillSlot = (line) => {
      const slot = slots.find((s) => s.dataset.slotKey === line.dataset.slot);
      if (slot) slot.classList.add("is-done");
    };
    if (!animate) {
      lines.forEach(fillSlot);
      replay.hidden = true;
      return;
    }
    const step = () => {
      if (index >= lines.length) return;
      const line = lines[index];
      line.classList.add("is-shown");
      fillSlot(line);
      chat.scrollTo({ top: chat.scrollHeight, behavior: "smooth" });
      index += 1;
      timer = setTimeout(step, line.classList.contains("bot__msg--in") ? 650 : 1050);
    };
    const start = () => {
      clearTimeout(timer);
      index = 0;
      lines.forEach((l) => l.classList.remove("is-shown"));
      slots.forEach((s) => s.classList.remove("is-done"));
      chat.classList.add("is-playing");
      step();
    };
    chat.classList.add("is-playing");
    replay.addEventListener("click", start);
    onceVisible(demo, start);
  });

  /* ---------- Magnetic primary call to action ---------- */
  if (finePointer.matches && animate) {
    document.querySelectorAll("[data-magnetic]").forEach((el) => {
      el.addEventListener("pointermove", (e) => {
        const r = el.getBoundingClientRect();
        el.style.transform = `translate(${((e.clientX - r.left - r.width / 2) * 0.22).toFixed(1)}px, ${((e.clientY - r.top - r.height / 2) * 0.32).toFixed(1)}px)`;
      });
      el.addEventListener("pointerleave", () => (el.style.transform = ""));
    });

    /* ---------- A soft light that follows the pointer ---------- */
    const glow = document.querySelector("[data-cursor]");
    if (glow) {
      let x = window.innerWidth / 2, y = window.innerHeight / 2, tx = x, ty = y, moving = false;
      const follow = () => {
        x += (tx - x) * 0.14;
        y += (ty - y) * 0.14;
        glow.style.transform = `translate(${x.toFixed(1)}px, ${y.toFixed(1)}px)`;
        if (Math.abs(tx - x) + Math.abs(ty - y) > 0.5) requestAnimationFrame(follow);
        else moving = false;
      };
      window.addEventListener("pointermove", (e) => {
        tx = e.clientX;
        ty = e.clientY;
        glow.classList.add("is-on");
        if (!moving) {
          moving = true;
          requestAnimationFrame(follow);
        }
      }, { passive: true });
    }
  }

  /* ---------- Section titles rise word by word ---------- */
  if (animate) {
    document.querySelectorAll(".section-title").forEach((title) => {
      const words = title.textContent.trim().split(/\s+/);
      title.textContent = "";
      words.forEach((word, i) => {
        const span = document.createElement("span");
        span.className = "word";
        span.style.setProperty("--i", String(i));
        span.textContent = word;
        title.append(span, i < words.length - 1 ? " " : "");
      });
    });
  }

  /* ---------- LED wall hero ---------- */
  // The name is drawn in LEDs. It boots with a sweep, drifts through colours, and dots
  // shy away from the pointer; a click sends a ripple across the wall. Paused off-screen.
  const wall = document.querySelector("[data-wall]");
  const wallCanvas = document.querySelector("[data-wall-canvas]");
  if (wall && wallCanvas && wallCanvas.getContext) {
    const ctx = wallCanvas.getContext("2d", { alpha: true });
    const copy = wall.querySelector("[data-wall-copy]");
    const stops = [[243, 181, 98], [255, 122, 69], [185, 163, 255], [141, 182, 255], [95, 211, 160], [243, 181, 98]];
    const hues = 48;
    const litStyles = Array.from({ length: hues }, (_, i) => {
      const p = (i / hues) * (stops.length - 1);
      const a = stops[Math.floor(p)];
      const b = stops[Math.ceil(p)];
      const f = p - Math.floor(p);
      return `rgb(${a.map((v, k) => Math.round(v + (b[k] - v) * f)).join(",")})`;
    });
    const dimLevels = 8;
    const dimStyles = Array.from({ length: dimLevels }, (_, i) => `rgba(243,181,98,${(0.05 + (i / (dimLevels - 1)) * 0.5).toFixed(3)})`);
    const litBuckets = litStyles.map(() => []);
    const dimBuckets = dimStyles.map(() => []);

    let w = 0, h = 0, gap = 9, cols = 0, rows = 0, mask = new Uint8Array(0);
    let pointer = { x: -9999, y: -9999, tx: -9999, ty: -9999 };
    let ripples = [];
    let running = false, visible = true, raf = 0, bootAt = performance.now();

    const rasterise = () => {
      mask = new Uint8Array(cols * rows);
      const lines = w < 640 ? ["HALİL", "MERT", "DEVELİ"] : ["HALİL MERT", "DEVELİ"];
      const wallRect = wall.getBoundingClientRect();
      const copyTop = copy.getBoundingClientRect().top - wallRect.top;
      const navRows = Math.ceil(84 / gap);
      const room = Math.max(8, Math.floor(copyTop / gap) - navRows - 1);
      const left = Math.round(copy.getBoundingClientRect().left / gap) + Math.round(parseFloat(getComputedStyle(copy).paddingLeft) / gap);
      const usable = cols - left * 2;

      const source = document.createElement("canvas");
      source.width = cols;
      source.height = rows;
      const sctx = source.getContext("2d", { willReadFrequently: true });
      const family = getComputedStyle(root).getPropertyValue("--font-sans");
      const widest = (size) => {
        sctx.font = `700 ${size}px ${family}`;
        return Math.max(...lines.map((l) => sctx.measureText(l).width));
      };
      const lineHeight = 1.02;
      let size = Math.floor(room / (lines.length * lineHeight));
      while (size > 6 && widest(size) > usable) size -= 1;
      sctx.font = `700 ${size}px ${family}`;
      sctx.fillStyle = "#fff";
      sctx.textBaseline = "alphabetic";
      const blockTop = navRows + Math.max(0, Math.floor((room - lines.length * size * lineHeight) / 2));
      lines.forEach((line, i) => sctx.fillText(line, left, blockTop + size * (i + 0.92) * lineHeight));
      const data = sctx.getImageData(0, 0, cols, rows).data;
      for (let i = 0; i < mask.length; i++) mask[i] = data[i * 4 + 3] > 110 ? 1 : 0;
    };

    const resize = () => {
      const rect = wall.getBoundingClientRect();
      const dpr = Math.min(window.devicePixelRatio || 1, 2);
      w = rect.width;
      h = rect.height;
      gap = w < 640 ? 7 : w < 1100 ? 8 : 9;
      wallCanvas.width = Math.round(w * dpr);
      wallCanvas.height = Math.round(h * dpr);
      ctx.setTransform(dpr, 0, 0, dpr, 0, 0);
      cols = Math.ceil(w / gap);
      rows = Math.ceil(h / gap);
      rasterise();
    };

    const draw = (now) => {
      const t = (now - bootAt) / 1000;
      const moving = animate;
      const sweep = moving ? Math.min(t / 1.5, 1.2) * cols : cols * 2;
      pointer.x += (pointer.tx - pointer.x) * 0.16;
      pointer.y += (pointer.ty - pointer.y) * 0.16;
      ripples = ripples.filter((r) => t - r.t < 1.6);

      litBuckets.forEach((b) => (b.length = 0));
      dimBuckets.forEach((b) => (b.length = 0));
      const litSize = gap * 0.7;
      const dimSize = Math.max(1.4, gap * 0.2);

      for (let row = 0; row < rows; row++) {
        const y = row * gap;
        for (let col = 0; col < cols; col++) {
          const x = col * gap;
          const lit = mask[row * cols + col] === 1 && col < sweep;
          let energy = 0, ox = 0, oy = 0;

          if (moving) {
            const dx = x - pointer.x;
            const dy = y - pointer.y;
            const d2 = dx * dx + dy * dy;
            if (d2 < 32400) {
              const d = Math.sqrt(d2) || 1;
              const f = 1 - d / 180;
              energy += f;
              if (lit) {
                ox = (dx / d) * f * 16;
                oy = (dy / d) * f * 16;
              }
            }
            for (let r = 0; r < ripples.length; r++) {
              const age = t - ripples[r].t;
              const d = Math.hypot(x - ripples[r].x, y - ripples[r].y);
              const ring = Math.abs(d - age * 620);
              if (ring < 46) energy += (1 - ring / 46) * (1 - age / 1.6) * 1.2;
            }
          }

          if (lit) {
            const hue = moving ? (col + row * 0.6) / cols * 0.7 + t * 0.045 : (col / cols) * 0.5;
            const index = Math.floor(((hue % 1) + 1) % 1 * hues);
            litBuckets[index].push(x + ox, y + oy);
          } else {
            let level = 0;
            if (moving) {
              const wave = Math.sin((x * 0.6 + y) * 0.012 - t * 0.9);
              level = Math.max(0, wave - 0.8) * 2.4 + energy * 0.9;
              if (Math.abs(col - sweep) < 2 && t < 1.5) level = 1;
            }
            const bucket = Math.min(dimLevels - 1, Math.floor(level * dimLevels));
            dimBuckets[bucket].push(x, y);
          }
        }
      }

      ctx.clearRect(0, 0, w, h);
      dimBuckets.forEach((points, i) => {
        if (!points.length) return;
        ctx.fillStyle = dimStyles[i];
        const size = dimSize + i * 0.35;
        for (let p = 0; p < points.length; p += 2) ctx.fillRect(points[p], points[p + 1], size, size);
      });
      litBuckets.forEach((points, i) => {
        if (!points.length) return;
        ctx.fillStyle = litStyles[i];
        for (let p = 0; p < points.length; p += 2) ctx.fillRect(points[p], points[p + 1], litSize, litSize);
      });
    };

    const loop = (now) => {
      draw(now);
      raf = requestAnimationFrame(loop);
    };
    const play = () => {
      if (running || reduceMotion.matches || !visible || document.hidden) return;
      running = true;
      raf = requestAnimationFrame(loop);
    };
    const pause = () => {
      running = false;
      cancelAnimationFrame(raf);
    };

    const boot = () => {
      resize();
      bootAt = performance.now();
      wall.classList.add("is-lit");
      draw(bootAt);
      play();
    };
    // The webfont changes the letter shapes, so rasterise after it has loaded.
    (document.fonts && document.fonts.ready ? document.fonts.ready : Promise.resolve()).then(boot);

    window.addEventListener("resize", () => {
      clearTimeout(resize.t);
      resize.t = setTimeout(() => {
        resize();
        draw(performance.now());
      }, 120);
    }, { passive: true });

    if (finePointer.matches) {
      wall.addEventListener("pointermove", (e) => {
        const rect = wall.getBoundingClientRect();
        pointer.tx = e.clientX - rect.left;
        pointer.ty = e.clientY - rect.top;
        if (pointer.x < -9000) {
          pointer.x = pointer.tx;
          pointer.y = pointer.ty;
        }
      });
      wall.addEventListener("pointerleave", () => {
        pointer.tx = pointer.ty = pointer.x = pointer.y = -9999;
      });
    }
    wall.addEventListener("pointerdown", (e) => {
      if (e.target.closest("a, button")) return;
      const rect = wall.getBoundingClientRect();
      ripples.push({ x: e.clientX - rect.left, y: e.clientY - rect.top, t: (performance.now() - bootAt) / 1000 });
    });

    if ("IntersectionObserver" in window) {
      new IntersectionObserver(([entry]) => {
        visible = entry.isIntersecting;
        visible ? play() : pause();
      }).observe(wall);
    }
    document.addEventListener("visibilitychange", () => (document.hidden ? pause() : play()));
  }
})();
